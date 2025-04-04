using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
namespace DerpRaven.IntegrationTests.DatabaseServiceTests;

public class CustomRequestServiceTests
{
    private CustomRequestService _customRequestService;
    private AppDbContext _context;
    private ProductType type1;
    private ProductType type2;
    private User user1;
    private User user2;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        var logger = Substitute.For<ILogger<CustomRequestService>>();
        _customRequestService = new CustomRequestService(_context, logger);

        type1 = new() { Name = "Plushie" };
        type2 = new() { Name = "Art" };
        _context.ProductTypes.Add(type1);
        _context.ProductTypes.Add(type2);

        user1 = new() { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        user2 = new() { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer" };
        _context.Users.Add(user1);
        _context.Users.Add(user2);

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
    public async Task CreateCustomRequest()
    {
        // Arrange
        var customRequest = new CustomRequestDto { Id = 1, UserId = 1, ProductTypeId = 1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };

        // Act
        await _customRequestService.CreateCustomRequestAsync(customRequest);
        var result = await _context.CustomRequests.FindAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Email.ShouldBe("test@example.com");
    }

    [Order(2)]
    [Test]
    public async Task GetAllCustomRequests_ShouldBeEmpty()
    {
        // Act
        var result = await _customRequestService.GetAllCustomRequestsAsync();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Order(3)]
    [Test]
    public async Task GetAllCustomRequests_ShouldReturn1()
    {
        // Arrange
        var customRequest = new CustomRequest { Id = 1, ProductType = type1, User = user1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetAllCustomRequestsAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result.Single().Email.ShouldBe("test@example.com");
    }

    [Order(4)]
    [Test]
    public async Task GetAllCustomRequests_ShouldReturn2()
    {
        // Arrange
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { ProductType = type1, User = user1, Status = "Pending", Email = "test1@example.com", Description = "I want a duckie." },
            new CustomRequest { ProductType = type2, User = user2, Status = "Completed", Email = "test2@example.com", Description = "I want a duckie." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetAllCustomRequestsAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.Any(c => c.Email == "test1@example.com").ShouldBeTrue();
        result.Any(c => c.Email == "test2@example.com").ShouldBeTrue();
    }

    [Order(5)]
    [Test]
    public async Task GetCustomRequestById_ShouldBeNull()
    {
        // Act
        var result = await _customRequestService.GetCustomRequestByIdAsync(1);

        // Assert
        result.ShouldBeNull();
    }

    [Order(6)]
    [Test]
    public async Task GetCustomRequestById_While1Added()
    {
        // Arrange
        var customRequest = new CustomRequest { ProductType = type1, User = user1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Email.ShouldBe("test@example.com");
    }

    [Order(7)]
    [Test]
    public async Task GetCustomRequestById_While2Added()
    {
        // Arrange
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { ProductType = type1, User = user1, Status = "Pending", Email = "test1@example.com", Description = "I want a duckie." },
            new CustomRequest { ProductType = type2, User = user2, Status = "Completed", Email = "test2@example.com", Description = "I want a duckie." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Email.ShouldBe("test1@example.com");
    }

    [Order(8)]
    [Test]
    public async Task GetCustomRequestByUserId_ShouldBeEmpty()
    {
        // Act
        var result = await _customRequestService.GetCustomRequestsByUserIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Order(9)]
    [Test]
    public async Task GetCustomRequestByUserId_While1Added()
    {
        // Arrange
        var customRequest = new CustomRequest { ProductType = type1, User = user1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestsByUserIdAsync(1);

        // Assert
        result.Count.ShouldBe(1);
        result.Single().Email.ShouldBe("test@example.com");
    }

    [Order(10)]
    [Test]
    public async Task GetCustomRequestByUserId_While2Added()
    {
        // Arrange
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { ProductType = type1, User = user1, Status = "Pending", Email = "test1@example.com", Description = "I want a duckie." },
            new CustomRequest { ProductType = type2, User = user2, Status = "Completed", Email = "test2@example.com", Description = "I want a duckie." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestsByUserIdAsync(1);

        // Assert
        result.Count.ShouldBe(1);
        result.Any(c => c.Email == "test1@example.com").ShouldBeTrue();
        result.Any(c => c.Email == "test2@example.com").ShouldBeFalse();
    }

    [Order(11)]
    [Test]
    public async Task GetCustomRequestByStatus_ShouldBeEmpty()
    {
        // Act
        var result = await _customRequestService.GetCustomRequestsByStatusAsync("Pending");

        // Assert
        result.ShouldBeEmpty();
    }

    [Order(12)]
    [Test]
    public async Task GetCustomRequestByStatus_While1Added()
    {
        // Arrange
        var customRequest = new CustomRequest { ProductType = type1, User = user1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestsByStatusAsync("Pending");

        // Assert
        result.Single().Email.ShouldBe("test@example.com");
    }

    [Order(13)]
    [Test]
    public async Task GetCustomRequestByStatus_While2Added()
    {
        // Arrange
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { ProductType = type1, User = user1, Status = "Pending", Email = "test1@example.com", Description = "I want a duckie." },
            new CustomRequest { ProductType = type2, User = user2, Status = "Completed", Email = "test2@example.com", Description = "I want a duckie." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestsByStatusAsync("Pending");

        // Assert
        result.Single().Email.ShouldBe("test1@example.com");
    }

    [Order(14)]
    [Test]
    public async Task GetCustomRequestByType_ShouldBeEmpty()
    {
        // Act
        var result = await _customRequestService.GetCustomRequestsByTypeAsync("Plushie");

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Order(15)]
    [Test]
    public async Task GetCustomRequestByType_While1Added()
    {
        // Arrange
        var customRequest = new CustomRequest { ProductType = type1, User = user1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestsByTypeAsync("Plushie");

        // Assert
        result.Single().Email.ShouldBe("test@example.com");
    }

    [Order(16)]
    [Test]
    public async Task GetCustomRequestByType_While2Added()
    {
        // Arrange
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, ProductType = type1, User = user1, Status = "Pending", Email = "test1@example.com", Description = "I want a duckie." },
            new CustomRequest { Id = 2, ProductType = type2, User = user2, Status = "Completed", Email = "test2@example.com", Description = "I want a duckie." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customRequestService.GetCustomRequestsByTypeAsync("Art");

        // Assert
        result.Single().Email.ShouldBe("test2@example.com");
    }

    [Order(17)]
    [Test]
    public async Task UpdateCustomRequest_ShouldFail()
    {
        // Act
        bool status = await _customRequestService.ChangeStatusAsync(1, "Completed");
        var result = await _context.CustomRequests.FindAsync(1);

        // Assert
        status.ShouldBeFalse();
        result.ShouldBeNull();
    }

    [Order(18)]
    [Test]
    public async Task UpdateCustomRequest_While1Added()
    {
        // Arrange
        var customRequest = new CustomRequest { Id = 1, ProductType = type1, User = user1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        // Act
        bool status = await _customRequestService.ChangeStatusAsync(1, "Complete");
        var result = await _context.CustomRequests.FindAsync(1);

        // Assert
        status.ShouldBeTrue();
        result.ShouldNotBeNull();
        result.Status.ShouldBe("Complete");
    }

    [Order(19)]
    [Test]
    public async Task UpdateCustomRequest_While2Added()
    {
        // Arrange
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, ProductType = type1, User = user1, Status = "Pending", Email = "test@example.com", Description = "I want a duckie." },
            new CustomRequest { Id = 2, ProductType = type2, User = user2, Status = "Completed", Email = "test@example.com", Description = "I want a duckie." }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        // Act
        bool status = await _customRequestService.ChangeStatusAsync(2, "Complete");
        var result = await _context.CustomRequests.FindAsync(2);

        // Assert
        status.ShouldBeTrue();
        result.ShouldNotBeNull();
        result.Status.ShouldBe("Complete");
    }
}
