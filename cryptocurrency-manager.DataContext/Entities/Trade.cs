using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Entities
{
    public enum TradeType
    {
        Buy,
        Sell
    }
    public class Trade: AbstractEntity
    {
        public int UserId { get; set; }
        public int CryptocurrencyId { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime TradeDate { get; set; }
        public TradeType TradeType { get; set; }


        public User User { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }
    }

}
