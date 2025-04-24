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

namespace cryptocurrency_manager.Services
{
    public interface ICryptoService
    {
        Task<List<CryptoDto>> GetAllCryptocurrenciesAsync();
        Task<CryptoDto> GetCryptocurrencyByIdAsync(int id);
        Task<CryptoDto> AddCryptocurrencyAsync(CryptoCreateDto cryptoCreateDto);
        Task<bool> DeleteCryptocurrencyAsync(int id);
        Task<CryptoDto> SetCryptocurrencyPrice(int id, decimal price);
        Task<CryptoHistoryDto> GetCryptocurrencyHistory(int id);
    }
    public class CryptoService : ICryptoService
    {
        private readonly CryptoDbContext _context;
        private readonly IMapper _mapper;
        public CryptoService(CryptoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<CryptoDto> AddCryptocurrencyAsync(CryptoCreateDto cryptoCreateDto)
        {
            var crypto = _mapper.Map<Cryptocurrency>(cryptoCreateDto);
            crypto.History = new List<History>();

            _context.Cryptocurrencies.Add(crypto);
            _context.SaveChanges();
            return Task.FromResult(_mapper.Map<CryptoDto>(crypto));
        }

        public async Task<bool> DeleteCryptocurrencyAsync(int id)
        {
            var crypto = await _context.Cryptocurrencies
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (crypto == null)
            {
                throw new Exception("Cryptocurrency not found");
            }
            crypto.IsDeleted = true;
            _context.Cryptocurrencies.Update(crypto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CryptoDto>> GetAllCryptocurrenciesAsync()
        {
            var cryptos = await _context.Cryptocurrencies
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return _mapper.Map<List<CryptoDto>>(cryptos);
        }

        public async Task<CryptoDto> GetCryptocurrencyByIdAsync(int id)
        {
            var crypto = await _context.Cryptocurrencies
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (crypto == null)
            {
                throw new Exception("Cryptocurrency not found");
            }
            return _mapper.Map<CryptoDto>(crypto);
        }

        public async Task<CryptoHistoryDto> GetCryptocurrencyHistory(int id)
        {
            var crypto = await _context.Cryptocurrencies
                .Where(c => !c.IsDeleted)
                .Include(c => c.History)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (crypto == null)
            {
                throw new Exception("Cryptocurrency not found");
            }
            return _mapper.Map<CryptoHistoryDto>(crypto);
        }

        public async Task<CryptoDto> SetCryptocurrencyPrice(int id, decimal price)
        {
            var crypto = await _context.Cryptocurrencies
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (crypto == null)
            {
                throw new Exception("Cryptocurrency not found");
            }
            crypto.Price = price;
            _context.Cryptocurrencies.Update(crypto);
            await _context.SaveChangesAsync();
            return _mapper.Map<CryptoDto>(crypto);

        }
    }
}
