using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shouldly;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
 
namespace DerpRaven.Tests.ServiceTests
{
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private AppDbContext _context;
 
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
 
            _context = new AppDbContext(options);
            var logger = Substitute.For<ILogger<OrderService>>();
            _orderService = new OrderService(_context, logger);
        }
 
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
 
        [Order(1)]
        [Test]
        public async Task CreateOrder()
        {
            var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
            var order = new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 };
 
            await _orderService.CreateOrderAsync(order);
            var result = await _context.Orders.FindAsync(1);
 
            result.ShouldBe(order);
        }
 
        [Order(2)]
        [Test]
        public async Task GetAllOrders()
        {
            var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer1" };
            var user2 = new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer2" };
            var orders = new List<Order>
            {
                new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 },
                new Order { Id = 2, Address = "456 Avenue", Email = "test2@example.com", OrderDate = DateTime.Now, User = user2 }
            };
            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();
 
            var retrievedOrders = await _orderService.GetAllOrdersAsync();
 
            retrievedOrders.ShouldBe(orders);
        }
 
        [Order(3)]
        [Test]
        public async Task GetOrderById()
        {
            var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer1" };
            var user2 = new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer2" };
            var orders = new List<Order>
            {
                new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 },
                new Order { Id = 2, Address = "456 Avenue", Email = "test2@example.com", OrderDate = DateTime.Now, User = user2 }
            };
            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();
 
            var retrievedOrders = await _orderService.GetOrderByIdAsync(1);
 
            retrievedOrders.ShouldBe(orders);
        }
 
        [Order(4)]
        [Test]
        public async Task GetOrdersByUser()
        {
            var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer1" };
            var user2 = new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer2" };
            var orders = new List<Order>
            {
                new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 },
                new Order { Id = 2, Address = "456 Avenue", Email = "test2@example.com", OrderDate = DateTime.Now, User = user2 }
            };
            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();
 
            var result = await _orderService.GetOrdersByUserIdAsync(1);
 
            result.ShouldBe(orders);
        }
 
        [Order(5)]
        [Test]
        public async Task UpdateOrder()
        {
            var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer1" };
            var order = new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
 
            order.Address = "456 Avenue";
            order.Email = "updated@example.com";
 
            await _orderService.UpdateOrderAsync(order);
            var result = await _context.Orders.FindAsync(1);
 
            result.ShouldBe(order);
        }
    }
}