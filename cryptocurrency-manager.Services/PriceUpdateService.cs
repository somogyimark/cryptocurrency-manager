using DataContext.Context;
using DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptocurrency_manager.Services
{
    public class PriceUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Random _random = new Random();

        public PriceUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();

                    var cryptoCurrencies = await _context.Cryptocurrencies.ToListAsync(stoppingToken);



                    var cryptos = _context.Cryptocurrencies.ToList();
                    foreach (var crypto in cryptos)
                    {
                        

                            decimal changePercent = (decimal)(_random.NextDouble() * 10 - 5);
                            crypto.Price += crypto.Price * changePercent / 100;
                            var history = new History
                            {
                                CryptocurrencyId = crypto.Id,
                                Price = crypto.Price,
                                Date = DateTime.UtcNow
                            };
                            _context.Cryptocurrencies.Update(crypto);
                            await _context.Histories.AddAsync(history);

                        
                    }
                    await _context.SaveChangesAsync(stoppingToken);
                    
                }
                Thread.Sleep(30000);
            }
        }
    }
}
