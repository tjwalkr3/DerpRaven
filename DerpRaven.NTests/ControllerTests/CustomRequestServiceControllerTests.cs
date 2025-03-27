using Castle.Core.Logging;
using DerpRaven.Api.Controllers;
using DerpRaven.Api.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using DerpRaven.Api.Model;
using static NUnit.Framework.Internal.OSPlatform;
namespace DerpRaven.Tests.ControllerTests;

public class CustomRequestServiceControllerTests
{
    ILogger _logger;

    public CustomRequestServiceControllerTests()
    {
        _logger = Substitute.For<ILogger>();
    }

    [Test]
    public async Task GetAllCustomRequests_ShouldReturnEmptyList_WhenNoRequestsExist()
    {
        // Arrange
        ICustomRequestService customRequestService = Substitute.For<ICustomRequestService>();
        customRequestService.GetAllCustomRequestsAsync().Returns([]);
        CustomRequestController controller = new(customRequestService);

        // Act
        var result = await controller.GetAllCustomRequests() as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var requests = result.Value as List<CustomRequestDto>;
        requests.ShouldNotBeNull();
        requests.ShouldBeEmpty();
    }

    [Test]
    public async Task GetAllCustomRequests_ShouldReturnSingleItem()
    {
        // Arrange
        ICustomRequestService customRequestService = Substitute.For<ICustomRequestService>();
        var requestDto = new CustomRequestDto()
            { Id = 1, Description = "", Email = "", ProductTypeId = 1, Status = "", UserId = 1 };
        var dtoList = new List<CustomRequestDto>() { requestDto };
        customRequestService.GetAllCustomRequestsAsync().Returns(dtoList);
        CustomRequestController controller = new(customRequestService);

        // Act
        var result = await controller.GetAllCustomRequests() as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var requests = result.Value as List<CustomRequestDto>;
        requests.ShouldNotBeEmpty();
        requests.Single().Id.ShouldBe(1);
    }

    [Test]
    public async Task GetAllCustomRequests_ShouldReturnTwoItems()
    {
        // Arrange
        ICustomRequestService customRequestService = Substitute.For<ICustomRequestService>();
        var dtoList = new List<CustomRequestDto>()
        {
            new() { Id = 1, Description = "", Email = "", ProductTypeId = 1, Status = "", UserId = 1 },
            new() { Id = 2, Description = "", Email = "", ProductTypeId = 1, Status = "", UserId = 1 }
        };
        customRequestService.GetAllCustomRequestsAsync().Returns(dtoList);
        CustomRequestController controller = new(customRequestService);

        // Act
        var result = await controller.GetAllCustomRequests() as OkObjectResult;

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
    public async Task GetCustomRequestById_ShouldReturnNotFound()
    {
        // Arrange
        ICustomRequestService customRequestService = Substitute.For<ICustomRequestService>();
        customRequestService.GetAllCustomRequestsAsync().Returns([]);
        CustomRequestController controller = new(customRequestService);

        // Act
        var result = await controller.GetCustomRequestById(10) as NoContentResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }
}
