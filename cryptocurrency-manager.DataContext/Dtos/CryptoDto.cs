using DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptocurrency_manager.DataContext.Dtos
{
    public class CryptoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }

    public class CryptoCreateDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }

    public class CryptoHistoryDto
    {
        public string Name { get; set; }
        public List<HistoryDto> Histories { get; set; }
    }

    public class CryptoProfitDto
    {
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Profit { get; set; }
    }

    public class CryptoProfitResponse
    {
        public List<CryptoProfitDto> CryptoProfits { get; set; }
        public decimal TotalProfit { get; set; }
    }
}