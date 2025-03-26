//using DerpRaven.Api.Model;
//using DerpRaven.Api.Services;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using NSubstitute;
//using Microsoft.EntityFrameworkCore.InMemory;
//using Shouldly;
//using DerpRaven.Api.Dtos;
//namespace DerpRaven.Tests.ServiceTests;

//public class UserServiceTests
//{
//    private UserService _userService;
//    private AppDbContext _context;

//    [SetUp]
//    public void Setup()
//    {
//        var options = new DbContextOptionsBuilder<AppDbContext>()
//            .UseInMemoryDatabase(databaseName: "TestDatabase")
//            .Options;

//        _context = new AppDbContext(options);
//        var logger = Substitute.For<ILogger<UserService>>();
//        _userService = new UserService(_context, logger);

//        _context.SaveChanges();
//    }

//    [TearDown]
//    public void TearDownAttribute()
//    {
//        _context.Dispose();
//    }

//    [Order(1)]
//    [Test]
//    public async Task GetAllUsers()
//    {
//        // Arrange
//        List<User> users = new()
//        {
//            new User { Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer", CustomRequests = [], Orders = [] },
//            new User { Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = false, Role = "customer", CustomRequests = [], Orders = [] }
//        };
//        await _context.Users.AddRangeAsync(users);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _userService.GetAllUsersAsync();

//        // Assert
//        result.ShouldNotBeNull();
//        result.Any(u => u.Name == "User1").ShouldBeTrue();
//        result.Any(u => u.Name == "User2").ShouldBeTrue();
//    }

//    [Order(2)]
//    [Test]
//    public async Task GetUserById()
//    {
//        // Arrange
//        var user = new User { Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer", CustomRequests = [], Orders = [] };
//        await _context.Users.AddAsync(user);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _userService.GetUserByIdAsync(1);

//        // Assert
//        result.ShouldNotBeNull();
//        result.Name.ShouldBe("User1");
//    }

//    [Order(3)]
//    [Test]
//    public async Task CreateUser()
//    {
//        // Arrange
//        var user = new UserDto() { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Role = "customer", Active = true };

//        // Act
//        var succeeded = await _userService.CreateUserAsync(user);
//        var result = _context.Products.Find(1);

//        // Assert
//        succeeded.ShouldBeTrue();
//        result.ShouldNotBeNull();
//        result.Name.ShouldBe("User1");
//    }

//    [Order(4)]
//    [Test]
//    public async Task UpdateUser()
//    {
//        // Arrange
//        var user = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer", CustomRequests = [], Orders = [] };
//        await _context.Users.AddAsync(user);
//        await _context.SaveChangesAsync();
//        var updatedUser = new UserDto { Id = 1, Name = "UpdatedUser", OAuth = "OAuth1", Email = "updated@example.com", Active = false, Role = "customer" };

//        // Act
//        await _userService.UpdateUserAsync(updatedUser);
//        var result = await _context.Users.FindAsync(1);

//        // Assert
//        result.ShouldNotBeNull();
//        result.Name.ShouldBe("UpdatedUser");
//        result.Email.ShouldBe("updated@example.com");
//        result.Active.ShouldBeFalse();
//    }

//    [Order(5)]
//    [Test]
//    public async Task GetUsersByStatus()
//    {
//        // Arrange
//        List<User> users = new()
//        {
//            new User { Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer", CustomRequests = [], Orders = [] },
//            new User { Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = false, Role = "customer", CustomRequests = [], Orders = [] }
//        };
//        await _context.Users.AddRangeAsync(users);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _userService.GetUsersByStatusAsync(true);

//        // Assert
//        result.ShouldNotBeNull();
//        result.Single().Active.ShouldBeTrue();
//        result.Single().Name.ShouldBe("User1");
//    }

//    [Order(6)]
//    [Test]
//    public async Task GetUserByEmail_WhileEmailsAreDifferent()
//    {
//        // Arrange
//        List<User> users = new()
//        {
//            new User { Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer", CustomRequests = [], Orders = [] },
//            new User { Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = false, Role = "customer", CustomRequests = [], Orders = [] }
//        };
//        await _context.Users.AddRangeAsync(users);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _userService.GetUsersByEmailAsync("user1@example.com");

//        // Assert
//        result.ShouldNotBeNull();
//        result.Count.ShouldBe(1);
//        result.Single().Email.ShouldBe("user1@example.com");
//    }

//    [Order(7)]
//    [Test]
//    public async Task GetUserByEmail_WhileTwoEmailsAreTheSame()
//    {
//        // Arrange
//        List<User> users = new()
//        {
//            new User { Name = "User1", OAuth = "OAuth1", Email = "user@example.com", Active = true, Role = "customer", CustomRequests = [], Orders = [] },
//            new User { Name = "User2", OAuth = "OAuth2", Email = "user@example.com", Active = false, Role = "customer", CustomRequests = [], Orders = [] }
//        };
//        await _context.Users.AddRangeAsync(users);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _userService.GetUsersByEmailAsync("user@example.com");

//        // Assert
//        result.ShouldNotBeNull();
//        result.Count.ShouldBe(2);
//        result.Any(u => u.Name == "User1").ShouldBeTrue();
//        result.Any(u => u.Name == "User2").ShouldBeTrue();
//    }

//    [Order(8)]
//    [Test]
//    public async Task GetUserByName()
//    {
//        // Arrange
//        var user = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer", CustomRequests = [], Orders = [] };
//        await _context.Users.AddAsync(user);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _userService.GetUsersByNameAsync("User1");

//        // Assert
//        result.ShouldNotBeNull();
//        result.Single().Name.ShouldBe("User1");
//    }
//}

