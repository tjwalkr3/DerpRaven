using DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using DerpRaven.Api;
namespace DerpRaven.UnitTests.ApiControllerTests;

public class CustomRequestServiceControllerTests
{
    CustomRequestController _controller;

    [SetUp]
    public void Setup()
    {
        ICustomRequestService service = Substitute.For<ICustomRequestService>();
        IDerpRavenMetrics metrics = Substitute.For<IDerpRavenMetrics>();
        IUserService userService = Substitute.For<IUserService>();
        _controller = new CustomRequestController(service, metrics, userService);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Test]
    public async Task GetCustomRequestsByUser_ShouldBeUnauthorized()
    {
        // Act
        var result = await _controller.GetCustomRequestsByUserEmail() as UnauthorizedResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status401Unauthorized);
    }
}