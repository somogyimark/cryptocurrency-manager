using AutoMapper;
using cryptocurrency_manager.DataContext.Dtos;
using DataContext.Context;
using DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace cryptocurrency_manager.Services
{
    public interface ITradeService
    {
        Task<bool> BuyCryptoAsync(int userId, int cryptoId, decimal amount);
        Task<bool> SellCryptoAsync(int userId, int cryptoId, decimal amount);
        Task<TradeDetailedDto> GetTransactionDetailsAsync(int transactionId);
        Task<IEnumerable<TradeDto>> GetUserTransactionsAsync(int userId);
    }
    public class TradeService : ITradeService
    {
        private readonly CryptoDbContext _cryptoDbContext;
        private readonly IMapper _mapper;

        public TradeService(CryptoDbContext cryptoDbContext, IMapper mapper)
        {
            _cryptoDbContext = cryptoDbContext;
            _mapper = mapper;
        }

        public async Task<bool> BuyCryptoAsync(int userId, int cryptoId, decimal amount)
        {
            using var transaction = await _cryptoDbContext.Database.BeginTransactionAsync();

            try
            {
                var user = await _cryptoDbContext.Users
                    .Include(u => u.Wallet)
                    .ThenInclude(w => w.Assets)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var crypto = await _cryptoDbContext.Cryptocurrencies
                    .FirstOrDefaultAsync(c => c.Id == cryptoId);

                if (user == null || crypto == null)
                    throw new Exception("User or cryptocurrency not found.");

                decimal totalPrice = crypto.Price * amount;

                if (user.Wallet.Balance < totalPrice)
                    throw new Exception("Insufficient balance.");


                user.Wallet.Balance -= totalPrice;

                crypto.Amount -= amount;
                if (crypto.Amount < 0)
                    throw new Exception("Not enough cryptocurrency available.");





                var asset = user.Wallet.Assets.FirstOrDefault(a => a.CryptoId == cryptoId);



                if (asset == null)
                {
                    asset = new Asset
                    {
                        CryptoId = cryptoId,
                        Cryptocurrency = crypto,
                        WalletId = user.Wallet.Id,
                        Amount = amount,
                        Price = crypto.Price,
                        TotalPrice = totalPrice,
                        Wallet = user.Wallet
                    };
                    _cryptoDbContext.Assets.Add(asset);
                }
                else
                {
                    asset.Amount += amount;
                }

                _cryptoDbContext.Transactions.Add(new Trade
                {
                    CryptocurrencyId = crypto.Id,
                    Price = crypto.Price,
                    TotalPrice = totalPrice,
                    TradeDate = DateTime.UtcNow,
                    Amount = amount,
                    UserId = user.Id,
                    TradeType = TradeType.Buy
                });

                await _cryptoDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> SellCryptoAsync(int userId, int cryptoId, decimal amount)
        {
            using var transaction = await _cryptoDbContext.Database.BeginTransactionAsync();

            try
            {
                var user = await _cryptoDbContext.Users
                    .Include(u => u.Wallet)
                        .ThenInclude(w => w.Assets)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var crypto = await _cryptoDbContext.Cryptocurrencies
                    .FirstOrDefaultAsync(c => c.Id == cryptoId);

                if (user == null || crypto == null)
                    throw new Exception("User or cryptocurrency not found.");

                var asset = user.Wallet.Assets
                    .FirstOrDefault(a => a.CryptoId == cryptoId);

                if (asset == null || asset.Amount < amount)
                    throw new Exception("Not enough cryptocurrency to sell.");

                decimal totalPrice = crypto.Price * amount;


                user.Wallet.Balance += totalPrice;
                
                crypto.Amount += amount;


                asset.Amount -= amount;


                if (asset.Amount == 0)
                {
                    _cryptoDbContext.Assets.Remove(asset);
                }

                _cryptoDbContext.Transactions.Add(new Trade
                {
                    CryptocurrencyId = crypto.Id,
                    Price = crypto.Price,
                    TotalPrice = totalPrice,
                    TradeDate = DateTime.UtcNow,
                    Amount = amount,
                    UserId = user.Id,
                    TradeType = TradeType.Sell
                });

                await _cryptoDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<TradeDto>> GetUserTransactionsAsync(int userId)
        {
            var transactions = await _cryptoDbContext.Transactions
                .Include(t => t.Cryptocurrency)
                .Where(t => t.UserId == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TradeDto>>(transactions);
        }

        public async Task<TradeDetailedDto> GetTransactionDetailsAsync(int transactionId)
        {
            var transaction = await _cryptoDbContext.Transactions
                .Include(t => t.Cryptocurrency)
                .FirstOrDefaultAsync(t => t.Id == transactionId);
            if (transaction == null)
                throw new Exception("Transaction not found.");
            return _mapper.Map<TradeDetailedDto>(transaction);
        }
    }
}
