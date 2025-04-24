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
        public List<HistoryDto> History { get; set; }
    }
}
