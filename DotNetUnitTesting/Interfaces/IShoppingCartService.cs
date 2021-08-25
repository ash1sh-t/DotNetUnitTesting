using DotNetUnitTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetUnitTesting.Interfaces
{
    public interface IShoppingCartService
    {
        IEnumerable<ShoppingItem> GetAllItems();
        ShoppingItem Add(ShoppingItem newItem);
        ShoppingItem GetById(Guid id);
        void Remove(ShoppingItem item);
    }
}
