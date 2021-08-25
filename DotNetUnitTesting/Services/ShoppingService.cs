using DotNetUnitTesting.Interfaces;
using DotNetUnitTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetUnitTesting.Services
{
    public class ShoppingService : IShoppingCartService
    {
        private readonly ShoppingContext _context;
        public ShoppingService(ShoppingContext context)
        {
            _context = context;
        }
        public ShoppingItem Add(ShoppingItem newItem)
        {
            _context.ShoppingItems.Add(newItem);
            _context.SaveChangesAsync();
            return newItem;
        }

        public IEnumerable<ShoppingItem> GetAllItems()
        {
            return _context.ShoppingItems.ToList();
        }

        public ShoppingItem GetById(Guid id)
        {
            return _context.ShoppingItems.Find(id);
        }

        public void Remove(ShoppingItem Item)
        {
            _context.ShoppingItems.Remove(Item);
            _context.SaveChangesAsync();
        }
    }
}
