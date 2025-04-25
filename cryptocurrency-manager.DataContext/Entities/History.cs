using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Entities
{
    public class History : AbstractEntity
    {
        public int CryptocurrencyId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }


        public Cryptocurrency Cryptocurrency { get; set; }
    }
}
