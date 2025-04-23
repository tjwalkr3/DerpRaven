using DerpRaven.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
namespace DerpRaven.UnitTests.ApiControllerTests;

public class HealthCheckControllerTests
{
    private HealthCheckController _controller;

    [SetUp]
    public void Setup()
    {
        _controller = new HealthCheckController();
    }

    [Test]
    public void GetHealthCheck_ReturnsOk()
    {
        var result = _controller.Get() as OkObjectResult;
        result.ShouldNotBeNull();
        var resultObject = result.Value as HealthCheckResult;
        resultObject?.Status.ShouldBe("Healthy");
    }
}

public class HealthCheckResult
{
    public string Status { get; set; } = string.Empty;
}
