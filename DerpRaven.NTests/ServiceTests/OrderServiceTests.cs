using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shouldly;
using NSubstitute;
namespace DerpRaven.Tests.ServiceTests;

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
        // Arrange
        var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        var order = new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 };

        // Act
        await _orderService.CreateOrderAsync(order);
        var result = await _context.Orders.FindAsync(1);

        // Assert
        result.ShouldBe(order);
    }

    [Order(2)]
    [Test]
    public async Task GetAllOrders()
    {
        // Arrange
        var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer1" };
        var user2 = new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer2" };
        var orders = new List<Order>
        {
            new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 },
            new Order { Id = 2, Address = "456 Avenue", Email = "test2@example.com", OrderDate = DateTime.Now, User = user2 }
        };
        await _context.Orders.AddRangeAsync(orders);
        await _context.SaveChangesAsync();

        // Act
        var retrievedOrders = await _orderService.GetAllOrdersAsync();

        // Assert
        retrievedOrders.ShouldBe(orders);
    }

    [Order(3)]
    [Test]
    public async Task GetOrderById()
    {
        // Arrange
        var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer1" };
        var user2 = new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer2" };
        var orders = new List<Order>
        {
            new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 },
            new Order { Id = 2, Address = "456 Avenue", Email = "test2@example.com", OrderDate = DateTime.Now, User = user2 }
        };
        await _context.Orders.AddRangeAsync(orders);
        await _context.SaveChangesAsync();

        // Act
        var retrievedOrder = await _orderService.GetOrderByIdAsync(1);

        // Assert
        retrievedOrder.ShouldBe(orders.Where(o => o.User.Id == 1).First());
    }

    [Order(4)]
    [Test]
    public async Task GetOrdersByUserId()
    {
        // Arrange
        var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer1" };
        var user2 = new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer2" };
        var orders = new List<Order>
        {
            new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 },
            new Order { Id = 2, Address = "456 Avenue", Email = "test2@example.com", OrderDate = DateTime.Now, User = user2 }
        };
        await _context.Orders.AddRangeAsync(orders);
        await _context.SaveChangesAsync();

        // Act
        var result = await _orderService.GetOrdersByUserIdAsync(1);

        // Assert
        result.ShouldBe(orders.Where(o => o.User.Id == 1));
    }

    [Order(5)]
    [Test]
    public async Task UpdateOrder()
    {
        // Arrange
        var user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer1" };
        var order = new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        order.Address = "456 Avenue";
        order.Email = "updated@example.com";

        // Act
        await _orderService.UpdateOrderAsync(order);
        var result = await _context.Orders.FindAsync(1);

        // Assert
        result.ShouldBe(order);
    }
}
