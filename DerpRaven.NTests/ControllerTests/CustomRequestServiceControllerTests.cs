using Castle.Core.Logging;
using DerpRaven.Api.Controllers;
using DerpRaven.Api.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
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
        {
            Id = 1,
            Description = "",
            Email = "",
            ProductTypeId = 1,
            Status = "",
            UserId = 1,
        };
        var dtoList = new List<CustomRequestDto>() { requestDto };
        customRequestService.GetAllCustomRequestsAsync().Returns(dtoList);
        CustomRequestController controller = new(customRequestService);

        // Act
        var result = await controller.GetAllCustomRequests() as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        var requests = result.Value as List<CustomRequestDto>;
        requests.ShouldNotBeEmpty();
        requests.Single().Id.ShouldBe(1);
    }
}
