using Microsoft.AspNetCore.Identity;
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
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }


        public List<Role> Roles { get; set; }
        public Wallet Wallet { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
    
    
}
