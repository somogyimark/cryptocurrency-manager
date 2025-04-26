using DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptocurrency_manager.DataContext.Dtos
{
    public class AssetDto
    {
        public int CryptoId { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}
