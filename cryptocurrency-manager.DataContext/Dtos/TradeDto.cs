using DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptocurrency_manager.DataContext.Dtos
{
    public class TradeDto
    {
        public int Id { get; set; }
        public int CryptocurrencyId { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalPrice { get; set; }
        public string TradeDate { get; set; }
        public TradeType TradeType { get; set; }
    }
    public class  TradeDetailedDto
    {
        public int UserId { get; set; }
        public CryptoDto Cryptocurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime TradeDate { get; set; }
        public TradeType TradeType { get; set; }
        
    }
}
