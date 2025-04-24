using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Context
{
    public class CryptoDbContext : DbContext
    {
        public CryptoDbContext(DbContextOptions<CryptoDbContext> options) : base(options)
        {
        }
        public DbSet<Entities.Cryptocurrency> Cryptocurrencies { get; set; }
        public DbSet<Entities.Asset> Assets { get; set; }
        public DbSet<Entities.History> Histories { get; set; }
        public DbSet<Entities.Transaction> Transactions { get; set; }
        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.Wallet> Wallets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add any additional configuration here
        }
    }
}
