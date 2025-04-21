using DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
namespace DerpRaven.UnitTests.ApiControllerTests;

public class UserServiceControllerTests
{
    UserController _controller;

    [SetUp]
    public void Setup()
    {
        IUserService userService = Substitute.For<IUserService>();
        var dtoList = new List<UserDto>()
        {
            new UserDto { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" },
            new UserDto { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer" },
        };

        userService.GetAllUsersAsync().Returns(dtoList);
        userService.GetUserByIdAsync(1).Returns(dtoList[0]);
        userService.CreateUserAsync(Arg.Any<UserDto>()).Returns(true);
        userService.UpdateUserAsync(Arg.Any<UserDto>()).Returns(true);
        userService.GetUserByEmailAsync("user1@example.com").Returns(dtoList.Where(u => u.Email == "user1@example.com").Single());

        _controller = new UserController(userService);
    }

    [Test]
    public async Task GetAllUsers()
    {
        // Act
        var result = await _controller.GetAllUsers() as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var users = result.Value as List<UserDto>;
        users.ShouldNotBeEmpty();
        users.Count.ShouldBe(2);
        users.Any(c => c.Id == 1).ShouldBeTrue();
        users.Any(c => c.Id == 2).ShouldBeTrue();
    }

    [Test]
    public async Task GetUserById()
    {
        // Act
        var result = await _controller.GetUserById(1) as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var user = result.Value as UserDto;
        user.ShouldNotBeNull();
        user.Id.ShouldBe(1);
    }

    [Test]
    public async Task GetUsersByEmail()
    {
        // Act
        var result = await _controller.GetUserByEmail("user1@example.com") as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var users = result.Value as UserDto;
        users.ShouldNotBeNull();
        users.Email.ShouldBe("user1@example.com");
    }

    [Test]
    public async Task CreateUser()
    {
        // Arrange
        var user = new UserDto()
        {
            Id = 1,
            Name = "User1",
            OAuth = "OAuth1",
            Email = "user1@example.com",
            Role = "customer",
            Active = true
        };

        // Act
        var result = await _controller.CreateUser(user) as CreatedResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status201Created);
    }

    [Test]
    public async Task UpdateUser()
    {
        //Act
        var result = await _controller.UpdateUser(1,
            new UserDto()
            {
                Id = 1,
                Name = "User1",
                OAuth = "OAuth1",
                Email = "user1@example.com",
                Role = "customer",
                Active = true
            }) as NoContentResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }
}
