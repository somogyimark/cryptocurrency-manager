using AutoMapper;
using cryptocurrency_manager.DataContext.Dtos;
using DataContext.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptocurrency_manager.Services
{
    public interface IWalletService
    {
        Task<WalletDto> GetWalletByUserIdAsync(int id);
        Task<PortfolioDto> GetPortfolioByUserIdAsync(int id);
        Task<bool> DeleteWalletAsync(int id);
        Task<WalletDto> UpdateWalletAsync(int id, WalletUpdateDto walletUpdateDto);
    }
    public class WalletService : IWalletService
    {
        private readonly CryptoDbContext _context;
        private readonly IMapper _mapper;
        public WalletService(CryptoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> DeleteWalletAsync(int id)
        {  
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == id);
            if (wallet == null)
            {
                throw new KeyNotFoundException("Wallet not found.");
            }

            
            wallet.Balance = 0;
            if (wallet.Assets != null)
            {
                wallet.Assets.Clear();
            }
            
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WalletDto> GetWalletByUserIdAsync(int id)
        {
            var wallet = await _context.Wallets
                .Include(w => w.Assets)
                .FirstOrDefaultAsync(w => w.UserId == id);
            if (wallet == null)
            {
                throw new KeyNotFoundException("Wallet not found.");
            }
            return _mapper.Map<WalletDto>(wallet);

        }


        public async Task<PortfolioDto> GetPortfolioByUserIdAsync(int id)
        {
            var wallet = await _context.Wallets
                .Include(w => w.Assets)
                .FirstOrDefaultAsync(w => w.UserId == id);
            if (wallet == null)
            {
                throw new KeyNotFoundException("Wallet not found.");
            }
            return _mapper.Map<PortfolioDto>(wallet);

        }

        public async Task<WalletDto> UpdateWalletAsync(int id, WalletUpdateDto walletUpdateDto)
        {
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == id);
            if (wallet == null)
            {
                throw new KeyNotFoundException("Wallet not found.");
            }
            wallet.Balance = walletUpdateDto.Balance;
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
            return _mapper.Map<WalletDto>(wallet);
        }
    }
}
