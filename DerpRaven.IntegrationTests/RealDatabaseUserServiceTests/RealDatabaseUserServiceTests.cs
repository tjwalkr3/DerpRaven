using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using DerpRaven.Shared.Dtos;
namespace DerpRaven.IntegrationTests.RealDatabaseServiceTests;


public class RealDatabaseUserServiceTests
{
    private UserService _userService;
    private AppDbContext _context;

    [SetUp]
    public void Setup()
    {
        string connection = "host=test-postgres-db;Database=postgres;Username=derp;Password=1234";
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connection).Options;
        _context = new AppDbContext(options);

        var logger = Substitute.For<ILogger<UserService>>();
        _userService = new UserService(_context, logger);
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
        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Any(u => u.Name == "Derp").ShouldBeTrue();
        result.Any(u => u.Name == "Derp2").ShouldBeTrue();
    }

    [Order(2)]
    [Test]
    public async Task GetUserById()
    {
        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Derp");
    }

    [Order(3)]
    [Test]
    public async Task CreateUser()
    {
        // Arrange
        var user = new UserDto() { Id = 3, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Role = "customer", Active = true };

        // Act
        var succeeded = await _userService.CreateUserAsync(user);
        var result = _context.Users.Find(3);

        // Assert
        succeeded.ShouldBeTrue();
        result.ShouldNotBeNull();
        result.Name.ShouldBe("User1");
    }

    [Order(4)]
    [Test]
    public async Task UpdateUser()
    {
        // Arrange
        var updatedUser = new UserDto { Id = 3, Name = "UpdatedUser", OAuth = "OAuth1", Email = "updated@example.com", Active = false, Role = "customer" };

        // Act
        await _userService.UpdateUserAsync(updatedUser);
        var result = await _context.Users.FindAsync(3);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("UpdatedUser");
        result.Email.ShouldBe("updated@example.com");
        result.Active.ShouldBeFalse();
    }

    [Order(5)]
    [Test]
    public async Task GetUsersByStatus()
    {
        // Act
        var result = await _userService.GetUsersByStatusAsync(true);

        // Assert
        result.ShouldNotBeNull();
        result.Single().Active.ShouldBeTrue();
        result.Single().Name.ShouldBe("Derp");
    }

    [Order(6)]
    [Test]
    public async Task GetUserByEmail()
    {
        // Act
        var result = await _userService.GetUserByEmailAsync("Derpipose@gmail.com");

        // Assert
        result.ShouldNotBeNull();
        result.Email.ShouldBe("Derpipose@gmail.com");
    }

    [Order(8)]
    [Test]
    public async Task GetUserByName()
    {
        // Act
        var result = await _userService.GetUsersByNameAsync("Derp");

        // Assert
        result.ShouldNotBeEmpty();
        result.Single().Name.ShouldBe("Derp");
    }


    [Order(9)]
    [Test]
    public async Task CheckEmailExists_ShouldBeTrue()
    {
        // Act
        bool exists = await _userService.EmailExistsAsync("Derpipose@gmail.com");

        // Assert
        exists.ShouldBeTrue();
    }

    [Order(10)]
    [Test]
    public async Task CheckEmailExists_ShouldBeFalse()
    {
        // Act
        bool exists = await _userService.EmailExistsAsync("user5@example.com");

        // Assert
        exists.ShouldBeFalse();
    }
}


