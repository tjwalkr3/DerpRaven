using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shouldly;
using NSubstitute;
using DerpRaven.Shared.Dtos;
namespace DerpRaven.IntegrationTests.DatabaseServiceTests;

public class OrderServiceTests
{
    private OrderService _orderService;
    private AppDbContext _context;
    private User user1;
    private User user2;
    private ProductType type1;
    private ProductType type2;
    List<OrderedProduct> orderedProducts = [];
    List<Order> orders = [];

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        var logger = Substitute.For<ILogger<OrderService>>();
        _orderService = new OrderService(_context, logger);

        type1 = new() { Name = "Plushie" };
        type2 = new() { Name = "Art" };
        _context.ProductTypes.Add(type1);
        _context.ProductTypes.Add(type2);

        user1 = new() { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        user2 = new() { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer" };
        _context.Users.Add(user1);
        _context.Users.Add(user2);

        orders = new()
        {
            new Order { Id = 1, Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1 },
            new Order { Id = 2, Address = "456 Avenue", Email = "test2@example.com", OrderDate = DateTime.Now, User = user2 }
        };
        _context.Orders.AddRange(orders);

        orderedProducts = new()
        {
            new() { Id = 1, Name = "Product1", Price = 10.0m, Quantity = 5, Order = orders[0]},
            new() { Id = 2, Name = "Product2", Price = 15.0m, Quantity = 5, Order = orders[1]}
        };
        _context.OrderedProducts.AddRange(orderedProducts);

        _context.SaveChanges();
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
        List<int> productIds = _context.OrderedProducts.Select(p => p.Id).ToList();
        var order = new OrderDto { Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, UserId = user1.Id, OrderedProductIds = productIds };

        // Act
        await _orderService.CreateOrderAsync(order);
        var result = await _context.Orders.FindAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Address.ShouldBe("123 Street");
    }

    [Order(2)]
    [Test]
    public async Task GetAllOrders()
    {
        // Act
        var retrievedOrders = await _orderService.GetAllOrdersAsync();

        // Assert
        retrievedOrders.Any(o => o.Address == "123 Street").ShouldBeTrue();
        retrievedOrders.Any(o => o.Address == "456 Avenue").ShouldBeTrue();
    }

    [Order(3)]
    [Test]
    public async Task GetOrderById()
    {
        // Act
        var result = await _orderService.GetOrderByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Address.ShouldBe("123 Street");
    }

    [Order(4)]
    [Test]
    public async Task GetOrdersByUserId()
    {
        // Act
        var result = await _orderService.GetOrdersByUserIdAsync(1);

        // Assert
        result.ShouldNotBeEmpty();
        result.Single().Address.ShouldBe("123 Street");
    }

    [Order(5)]
    [Test]
    public async Task UpdateOrder()
    {
        // Act
        await _orderService.UpdateOrderAsync(1, "456 Avenue", "updated@example.com");
        var result = await _context.Orders.FindAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Address.ShouldBe("456 Avenue");
        result.Email.ShouldBe("updated@example.com");
    }
}
