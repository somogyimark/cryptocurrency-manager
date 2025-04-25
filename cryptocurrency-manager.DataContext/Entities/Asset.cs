using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Entities
{
    public class Asset: AbstractEntity
    {
        public int CryptoId { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }


        public Cryptocurrency Cryptocurrency { get; set; }
        public Wallet Wallet { get; set; }


    }
}
