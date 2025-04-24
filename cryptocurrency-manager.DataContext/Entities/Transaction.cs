using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Entities
{
    public enum TransactionType
    {
        Buy,
        Sell,
        Transfer
    }
    public class Transaction: AbstractEntity
    {
        public int UserId { get; set; }
        public int CryptocurrencyId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; } // e.g., "buy", "sell"


        public User User { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }
    }

}
