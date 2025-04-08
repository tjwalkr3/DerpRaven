using DerpRaven.Api.Controllers;
using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
namespace DerpRaven.UnitTests.ApiControllerTests;

public class CustomRequestServiceControllerTests
{
    CustomRequestController _controller;

    [SetUp]
    public void Setup()
    {
        _controller = new(Substitute.For<ICustomRequestService>());
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