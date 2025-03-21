using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using System.Data;
namespace DerpRaven.Tests.ServiceTests;

public class CustomRequestServiceTests
{
    private CustomRequestService _customRequestService;
    private AppDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        var logger = Substitute.For<ILogger<CustomRequestService>>();
        _customRequestService = new CustomRequestService(_context, logger);

        _context.ProductTypes.Add(new() { Name = "Plushie" });
        _context.ProductTypes.Add(new() { Name = "Art" });
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Order(1)]
    [Test]
    public async Task CreateCustomRequest()
    {
        // Arrange
        var customRequest = new CustomRequest { Id = 1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };

        // Act
        await _customRequestService.CreateCustomRequest(customRequest);
        var result = await _context.CustomRequests.FindAsync(1);

        // Assert
        result.ShouldBe(customRequest);
    }

    [Order(2)]
    [Test]
    public async Task GetAllCustomRequests()
    {
        // Arrange
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." },
            new CustomRequest { Id = 2, Status = "Completed", Email = "test@example.com", Description = "I want a duckie." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetAllCustomRequestsAsync();

        // Assert
        result.ShouldBe(customRequests);
    }

    [Order(3)]
    [Test]
    public async Task GetCustomRequestById()
    {
        // Arrange
        var customRequest = new CustomRequest { Id = 1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestById(1);

        // Assert
        result.ShouldBe(customRequest);
    }

    [Order(4)]
    [Test]
    public async Task GetCustomRequestByUser()
    {
        // Arrange
        User user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        User user2 = new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer" };
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, Status = "Pending", User = user1, Email = "test@example.com", Description = "I want a duckie." },
            new CustomRequest { Id = 2, Status = "Completed", User = user2, Email = "test2@example.com", Description = "I want a duckie 2." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestsByUserAsync(1);

        // Assert
        result.ShouldBe(customRequests.Where(c => c.User.Id == 1));
    }

    [Order(5)]
    [Test]
    public async Task GetCustomRequestByStatus()
    {
        // Arrange
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." },
            new CustomRequest { Id = 2, Status = "Pending", Email = "test2@example.com", Description = "I want 2 duckies." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestsByStatusAsync("Pending");

        // Assert
        result.ShouldBe(customRequests);
    }

    [Order(6)]
    [Test]
    public async Task GetCustomRequestByType()
    {
        // Arrange
        ProductType type1 = new() { Name = "Plushie" };
        ProductType type2 = new() { Name = "Art" };

        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, ProductType = type1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." },
            new CustomRequest { Id = 2, ProductType = type2, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestsByTypeAsync("Plushie");

        // Assert
        result.ShouldBe(customRequests.Where(c => c.ProductType.Name == "Plushie"));
    }

    [Order(7)]
    [Test]
    public async Task UpdateCustomRequest()
    {
        // Arrange
        var customRequest = new CustomRequest { Id = 1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        customRequest.Status = "Completed";

        // Act
        await _customRequestService.ChangeStatus(customRequest.Id, customRequest.Status);
        var result = await _context.CustomRequests.FindAsync(1);

        // Assert
        result.ShouldBe(customRequest);
    }
}
