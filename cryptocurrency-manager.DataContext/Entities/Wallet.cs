﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Entities
{
    public class Wallet : AbstractEntity
    {
        public decimal Balance { get; set; }
        public List<Asset> Assets { get; set; }
        public int UserId { get; set; }



        public User User { get; set; }


    }
}
