using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataContext.Entities
{
    public class User : AbstractEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Navigation properties
        public Wallet Wallet { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
    
    
}
