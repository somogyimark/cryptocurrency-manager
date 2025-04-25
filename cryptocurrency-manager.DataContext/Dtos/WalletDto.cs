using DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptocurrency_manager.DataContext.Dtos
{
    public class WalletDto
    {
        public decimal Balance { get; set; }
        public List<Asset> Assets { get; set; }
    }

    public class WalletUpdateDto
    {
        public decimal Balance { get; set; }
    }
}
