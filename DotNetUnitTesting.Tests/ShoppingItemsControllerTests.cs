using DotNetUnitTesting.Controllers;
using DotNetUnitTesting.Interfaces;
using DotNetUnitTesting.Models;
using DotNetUnitTesting.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace DotNetUnitTesting.Tests
{
    public class ShoppingItemsControllerTests
    {
        ShoppingItemsController _controller;
        IShoppingCartService _service;
        ShoppingContext _context;

        public ShoppingItemsControllerTests()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ShoppingContext>();

            builder.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                    .UseInternalServiceProvider(serviceProvider);

            _context = new ShoppingContext(builder.Options);
            _context.Database.Migrate();
            _service = new ShoppingService(_context);
            _controller = new ShoppingItemsController(_service);

        }
        [Fact]
        public void GetAllItems()
        {
            //Arrange

            //Act
            var result = _controller.GetShoppingItems();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;
            Assert.IsType<List<ShoppingItem>>(list.Value);

            var listItems = list.Value as List<ShoppingItem>;
            Assert.Equal(7, listItems.Count);
        }
        [Theory]
        [InlineData("e5483702-7a26-4d96-f5e7-08d967c3ff90", "e5483702-7a26-4d96-f5e7-08d967c3ff11")]
        public void GetItemByIdTest(string Guid1, string Guid2)
        {
            //Arrange
            var ValidGuid = new Guid(Guid1);
            var InvalidGuid = new Guid(Guid2);
            //Act
            var ValidResult = _controller.GetShoppingItem(ValidGuid);
            var InvalidResult = _controller.GetShoppingItem(InvalidGuid);
            //Assert
            Assert.IsType<NotFoundResult>(InvalidResult.Result);
            Assert.IsType<OkObjectResult>(ValidResult.Result);

            var ValidItem = ValidResult.Result as OkObjectResult;
            Assert.IsType<ShoppingItem>(ValidItem.Value);

            var ShoppingItem = ValidItem.Value as ShoppingItem;
            Assert.Equal("Butter", ShoppingItem.Name);
            Assert.Equal("Amul", ShoppingItem.Manufacturer);

        }
        [Fact]
        public void PostShoppingItemTest()
        {
            //Arrange
            var CompleteItem = new ShoppingItem()
            {
                Name = "Water Bottle",
                Price = (decimal)39.99,
                Manufacturer = "Milton"
            };
            //Act
            var ValidResult = _controller.PostShoppingItem(CompleteItem);

            //Assert
            Assert.IsType<CreatedAtActionResult>(ValidResult);

            var Item = ValidResult as CreatedAtActionResult;
            Assert.IsType<ShoppingItem>(Item.Value);

            var ShoppingItem = Item.Value as ShoppingItem;
            Assert.Equal(CompleteItem.Name, ShoppingItem.Name);
            Assert.Equal(CompleteItem.Price, ShoppingItem.Price);
            Assert.Equal(CompleteItem.Manufacturer, ShoppingItem.Manufacturer);

            _controller.DeleteShoppingItem(ShoppingItem.Id);

            //Arrange
            var InCompleteItem = new ShoppingItem()
            {
                Price = (decimal)39.99,
                Manufacturer = "Milton"
            };
            _controller.ModelState.AddModelError("Name", "The Name field is required");
            //Act
            var BadResponse = _controller.PostShoppingItem(InCompleteItem);
            //Assert
            Assert.IsType<BadRequestObjectResult>(BadResponse);
        }
        [Theory]
        [InlineData("e5483702-7a26-4d96-f5e7-08d967c3ff90", "e5483702-7a26-4d96-f5e7-08d967c3ff11")]
        public void DeleteShoppingItemTest(string Guid1, string Guid2)
        {
            //Arrange
            var ValidGuid = new Guid(Guid1);
            var InvalidGuid = new Guid(Guid2);
            //Act
            var ValidResult = _controller.DeleteShoppingItem(ValidGuid);
            //Assert
            Assert.IsType<OkResult>(ValidResult);

            //Act
            var InvalidResult = _controller.DeleteShoppingItem(InvalidGuid);
            //Assert
            Assert.IsType<NotFoundResult>(InvalidResult);

        }

        //  [     {
        //  "id": "c00061f2-6c37-47b0-2837-08d967b4811c",
        //  "name": "Bread",
        //  "price": 12,
        //  "manufacturer": "Treat"
        //},
        //{
        //  "id": "e5483702-7a26-4d96-f5e7-08d967c3ff90",
        //  "name": "Butter",
        //  "price": 55.5,
        //  "manufacturer": "Amul"
        //},
        //{
        //  "id": "067eebc2-5399-4ae0-f5e8-08d967c3ff90",
        //  "name": "Chips",
        //  "price": 35.5,
        //  "manufacturer": "Lays"
        //}]
    }
}
