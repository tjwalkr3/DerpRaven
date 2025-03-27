using DerpRaven.Api.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderServiceController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderServiceController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(int userId)
    {
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderDto order)
    {
        var createdOrder = await _orderService.CreateOrderAsync(order);
        return Created();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, string address, string email)
    {
        bool wasUpdated = await _orderService.UpdateOrderAsync(id, address, email);
        if (wasUpdated) return BadRequest();
        return NoContent();
    }
}
