using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Entities
{
    public class Wallet : AbstractEntity
    {
        public decimal Balance { get; set; } = 1000; // Default balance
        public List<Asset> Assets { get; set; }

    }
}
