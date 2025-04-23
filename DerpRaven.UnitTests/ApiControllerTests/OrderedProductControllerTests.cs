using DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
namespace DerpRaven.UnitTests.ApiControllerTests;

public class OrderedProductControllerTests
{
    // Test only the returned http status codes
    OrderedProductController _controller;
    [SetUp]
    public void Setup()
    {
        IOrderedProductService orderedProductService = Substitute.For<IOrderedProductService>();
        orderedProductService.GetOrderedProductsByOrderId(Arg.Any<int>()).Returns(new List<OrderedProductDto>());
        orderedProductService.CreateOrderedProducts(Arg.Any<List<OrderedProductDto>>()).Returns(true);
        _controller = new OrderedProductController(orderedProductService);
    }

    [Test]
    public async Task GetOrderedProductsByOrderId_ShouldBeBadRequest()
    {
        // Act
        var result = await _controller.GetOrderedProductsByOrderId(-10) as BadRequestObjectResult;

        // Assert
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task GetOrderedProductsByOrderId_ShouldBeCreatedRequest()
    {
        // Act
        var result = await _controller.GetOrderedProductsByOrderId(1) as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task CreateOrderedProducts_ShouldBeBadRequest()
    {
        // Act
        var result = await _controller.CreateOrderedProducts(new List<OrderedProductDto>()) as BadRequestObjectResult;
        // Assert
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task CreateOrderedProducts_ShouldBeCreated()
    {
        // Arrange
        var orderedProducts = new List<OrderedProductDto> { new OrderedProductDto() };

        // Act
        var result = await _controller.CreateOrderedProducts(orderedProducts) as CreatedResult;

        // Assert
        result.ShouldNotBeNull();
    }
}
