using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetUnitTesting.Models;
using DotNetUnitTesting.Interfaces;

namespace DotNetUnitTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingItemsController : ControllerBase
    {
        //private readonly ShoppingContext _context;
        private readonly IShoppingCartService _service;

        public ShoppingItemsController(IShoppingCartService service)
        {
            _service = service;
        }

        // GET: api/ShoppingItems
        [HttpGet]
        public ActionResult<IEnumerable<ShoppingItem>> GetShoppingItems()
        {
            var items = _service.GetAllItems();
            return Ok(items);
            //return await _context.ShoppingItems.ToListAsync();
        }

        // GET: api/ShoppingItems/5
        [HttpGet("{id}")]
        public ActionResult<ShoppingItem> GetShoppingItem(Guid id)
        {
            var shoppingItem = _service.GetById(id);

            //var shoppingItem = await _context.ShoppingItems.FindAsync(id);

            if (shoppingItem == null)
            {
                return NotFound();
            }

            return Ok(shoppingItem);
        }

        // POST: api/ShoppingItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostShoppingItem([FromBody] ShoppingItem SItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = _service.Add(SItem);
            return CreatedAtAction("GetShoppingItem", new { id = item.Id }, item);
        }
        // DELETE: api/ShoppingItems/5
        [HttpDelete("{id}")]
        public ActionResult DeleteShoppingItem(Guid id)
        {
            var existingItem = _service.GetById(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            _service.Remove(existingItem);
            return Ok();
        }
    }
}
