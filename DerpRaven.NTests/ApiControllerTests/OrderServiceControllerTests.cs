using DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;

namespace DerpRaven.NTests.ApiControllerTests;

public class OrderServiceControllerTests
{
    OrderController _controller;

    [SetUp]
    public void Setup()
    {
        IOrderService orderService = Substitute.For<IOrderService>();
        var dtoList = new List<OrderDto>()
        {
            new() { Id = 1, Address = "address1", Email = "email1", UserId = 1 },
            new() { Id = 2, Address = "address2", Email = "email2", UserId = 2 }
        };
        orderService.GetAllOrdersAsync().Returns(dtoList);
        orderService.GetOrderByIdAsync(1).Returns(dtoList[0]);
        orderService.GetOrdersByUserIdAsync(2).Returns(dtoList.Where(c => c.UserId == 2).ToList());
        orderService.CreateOrderAsync(Arg.Any<OrderDto>()).Returns(true);
        orderService.UpdateOrderAsync(1, "new address", "new email").Returns(true);

        _controller = new(orderService);
    }

    [Test]
    public async Task GetAllOrders()
    {
        // Act
        var result = await _controller.GetAllOrders() as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var orders = result.Value as List<OrderDto>;
        orders.ShouldNotBeEmpty();
        orders.Count.ShouldBe(2);
        orders.Any(c => c.Id == 1).ShouldBeTrue();
        orders.Any(c => c.Id == 2).ShouldBeTrue();
    }

    [Test]
    public async Task GetOrderById()
    {
        // Act
        var result = await _controller.GetOrderById(1) as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var order = result.Value as OrderDto;
        order.ShouldNotBeNull();
        order.Id.ShouldBe(1);
    }

    [Test]
    public async Task GetOrdersByUserId()
    {
        // Act
        var result = await _controller.GetOrdersByUserId(2) as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var orders = result.Value as List<OrderDto>;
        orders.ShouldNotBeEmpty();
        orders.Count.ShouldBe(1);
        orders.Any(c => c.UserId == 2).ShouldBeTrue();
    }

    [Test]
    public async Task CreateOrder()
    {
        // Arrange
        var order = new OrderDto() { Id = 3, Address = "address3", Email = "email3", UserId = 3 };

        // Act
        var result = await _controller.CreateOrder(order) as CreatedResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status201Created);
    }

    [Test]
    public async Task UpdateOrder()
    {
        // Act
        var result = await _controller.UpdateOrder(1, "new address", "new email") as NoContentResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }



}
