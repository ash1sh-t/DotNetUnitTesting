using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetUnitTesting.Models
{
    public class ShoppingContext :DbContext
    {
        public ShoppingContext(DbContextOptions Options) : base(Options) { }

        public DbSet<ShoppingItem> ShoppingItems { get; set; }
    }
}
