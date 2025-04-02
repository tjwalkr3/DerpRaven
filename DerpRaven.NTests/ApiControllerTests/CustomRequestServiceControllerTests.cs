using DerpRaven.Api.Controllers;
using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
namespace DerpRaven.Tests.ApiControllerTests;

public class CustomRequestServiceControllerTests
{
    CustomRequestController _controller;

    [SetUp]
    public void Setup()
    {
        ICustomRequestService customRequestService = Substitute.For<ICustomRequestService>();
        var dtoList = new List<CustomRequestDto>()
        {
            new() { Id = 1, Description = "description1", Email = "email1", ProductTypeId = 1, Status = "Pending", UserId = 1 },
            new() { Id = 2, Description = "description2", Email = "email2", ProductTypeId = 2, Status = "Denied", UserId = 2 }
        };
        customRequestService.GetAllCustomRequestsAsync().Returns(dtoList);
        customRequestService.GetCustomRequestByIdAsync(1).Returns(dtoList[0]);
        customRequestService.GetCustomRequestsByUserIdAsync(1).Returns(dtoList);
        customRequestService.GetCustomRequestsByStatusAsync("Pending").Returns(dtoList.Where(c => c.Status == "Pending").ToList());
        customRequestService.GetCustomRequestsByTypeAsync("type1").Returns(dtoList.Where(c => c.ProductTypeId == 1).ToList());
        customRequestService.CreateCustomRequestAsync(Arg.Any<CustomRequestDto>()).Returns(true);
        customRequestService.ChangeStatusAsync(1, "Approved").Returns(true);
        _controller = new(customRequestService);
    }

    [Test]
    public async Task GetAllCustomRequests()
    {
        // Act
        var result = await _controller.GetAllCustomRequests() as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var requests = result.Value as List<CustomRequestDto>;
        requests.ShouldNotBeEmpty();
        requests.Count.ShouldBe(2);
        requests.Any(c => c.Id == 1).ShouldBeTrue();
        requests.Any(c => c.Id == 2).ShouldBeTrue();
    }

    [Test]
    public async Task GetCustomRequestById()
    {
        // Act
        var result = await _controller.GetCustomRequestById(1) as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var request = result.Value as CustomRequestDto;
        request.ShouldNotBeNull();
        request.Id.ShouldBe(1);
    }

    [Test]
    public async Task GetCustomRequestsByUser()
    {
        // Act
        var result = await _controller.GetCustomRequestsByUser(1) as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var requests = result.Value as List<CustomRequestDto>;
        requests.ShouldNotBeEmpty();
        requests.Count.ShouldBe(2);
        requests.Any(c => c.Id == 1).ShouldBeTrue();
        requests.Any(c => c.Id == 2).ShouldBeTrue();
    }

    [Test]
    public async Task GetCustomRequestsByStatus()
    {
        // Act
        var result = await _controller.GetCustomRequestsByStatus("Pending") as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var requests = result.Value as List<CustomRequestDto>;
        requests.ShouldNotBeEmpty();
        requests.Count.ShouldBe(1);
        requests.Any(c => c.Id == 1).ShouldBeTrue();
    }

    [Test]
    public async Task GetCustomRequestsByType()
    {
        // Act
        var result = await _controller.GetCustomRequestsByType("type1") as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var requests = result.Value as List<CustomRequestDto>;
        requests.ShouldNotBeEmpty();
        requests.Count.ShouldBe(1);
        requests.Any(c => c.Id == 1).ShouldBeTrue();
    }

    [Test]
    public async Task CreateCustomRequest()
    {
        // Arrange
        var request = new CustomRequestDto { Id = 3, Description = "description3", Email = "email3", ProductTypeId = 3, Status = "Pending", UserId = 3 };
        // Act
        var result = await _controller.CreateCustomRequest(request) as CreatedResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status201Created);
    }

    [Test]
    public async Task ChangeStatus()
    {
        // Act
        var result = await _controller.ChangeStatus(1, "Approved") as NoContentResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }
}