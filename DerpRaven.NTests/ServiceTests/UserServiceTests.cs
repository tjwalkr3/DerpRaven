using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Microsoft.EntityFrameworkCore.InMemory;
using Shouldly;

namespace DerpRaven.Tests.ServiceTests;

public class UserServiceTests
{
    AppDbContext _context;
    ILogger<UserService> _logger;
    UserService _userService;

    [SetUp]
    public void Setup()
    {

        _logger = Substitute.For<ILogger<UserService>>();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _userService = new UserService(_context, _logger);
    }

    [TearDown]
    public void TearDownAttribute()
    {
        _context.Dispose();
    }

    [Order(1)]
    [Test]
    public async Task GetAllUsers()
    {
        // Arrange
        _context.Users.Add(new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" });
        _context.Users.Add(new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = false, Role = "customer" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
    }

    [Order(2)]
    [Test]
    public async Task GetUserById()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result?.Id.ShouldBe(1);
    }

    [Order(3)]
    [Test]
    public async Task CreateUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };

        // Act
        var result = await _userService.CreateUserAsync(user);

        // Assert
        _context.Users.ShouldContain(user);
        result.ShouldNotBeNull();
        result?.Id.ShouldBe(1);
    }

    [Order(4)]
    [Test]
    public async Task UpdateUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var updatedUser = new User { Id = 1, Name = "UpdatedUser", OAuth = "OAuth1", Email = "updated@example.com", Active = false, Role = "customer" };

        // Act
        await _userService.UpdateUserAsync(updatedUser);

        // Assert
        var result = await _context.Users.FindAsync(1);
        result.ShouldNotBeNull();
        result?.Name.ShouldBe("UpdatedUser");
        result?.Email.ShouldBe("updated@example.com");
        result?.Active.ShouldBeFalse();
    }

    [Order(5)]
    [Test]
    public async Task GetUsersByStatus()
    {
        // Arrange
        _context.Users.Add(new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" });
        _context.Users.Add(new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = false, Role = "customer" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUsersByStatusAsync(true);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
        result.First().Active.ShouldBeTrue();
    }

    [Order(6)]
    [Test]
    public async Task GetUserByEmail()
    {
        // Arrange
        _context.Users.Add(new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer1" });
        _context.Users.Add(new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer2" });
        _context.Users.Add(new User { Id = 3, Name = "User3", OAuth = "OAuth3", Email = "user2@example.com", Active = false, Role = "custome3" });
        await _context.SaveChangesAsync();

        // Act
        string email = "user2@example.com";
        var result = await _userService.GetUsersByEmailAsync(email);

        // Assert
        result.ShouldNotBeNull();
        result.Where(u => u.Email == email).Count().ShouldBe(2);
        var filtered = _context.Users.Where(u => u.Email == email);
        filtered.Contains(_context.Users.Find(2)).ShouldBeTrue();
        filtered.Contains(_context.Users.Find(3)).ShouldBeTrue();
    }

    [Order(7)]
    [Test]
    public async Task GetUserByName()
    {
        // Arrange
        _context.Users.Add(new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUserByNameAsync("User1");

        // Assert
        result.ShouldNotBeNull();
        result?.Name.ShouldBe("User1");
    }
}

