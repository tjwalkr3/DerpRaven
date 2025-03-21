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
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
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
    public async Task GetUserById_ShouldReturnUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUserById(1);

        // Assert
        result.ShouldNotBeNull();
        result?.Id.ShouldBe(1);
    }

    [Order(3)]
    [Test]
    public async Task CreateUser_ShouldAddUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };

        // Act
        var result = await _userService.CreateUser(user);

        // Assert
        _context.Users.ShouldContain(user);
        result.ShouldNotBeNull();
        result?.Id.ShouldBe(1);
    }

    [Order(4)]
    [Test]
    public async Task UpdateUser_ShouldUpdateExistingUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var updatedUser = new User { Id = 1, Name = "UpdatedUser", OAuth = "OAuth1", Email = "updated@example.com", Active = false, Role = "customer" };

        // Act
        await _userService.UpdateUser(updatedUser);

        // Assert
        var result = await _context.Users.FindAsync(1);
        result.ShouldNotBeNull();
        result?.Name.ShouldBe("UpdatedUser");
        result?.Email.ShouldBe("updated@example.com");
        result?.Active.ShouldBeFalse();
    }

    [Order(5)]
    [Test]
    public async Task GetUsersByStatus_ShouldReturnUsersWithGivenStatus()
    {
        // Arrange
        _context.Users.Add(new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" });
        _context.Users.Add(new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = false, Role = "customer" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUsersByStatus(true);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
        result.First().Active.ShouldBeTrue();
    }

    [Order(6)]
    [Test]
    public async Task GetUserByEmail_ShouldReturnUsersWithGivenEmail()
    {
        // Arrange
        _context.Users.Add(new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" });
        _context.Users.Add(new User { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = false, Role = "customer" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUserByEmail("user1@example.com");

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
        result.First().Email.ShouldBe("user1@example.com");
    }

    [Order(7)]
    [Test]
    public async Task GetUserByName_ShouldReturnUserWithGivenName()
    {
        // Arrange
        _context.Users.Add(new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUserByName("User1");

        // Assert
        result.ShouldNotBeNull();
        result?.Name.ShouldBe("User1");
    }
}

