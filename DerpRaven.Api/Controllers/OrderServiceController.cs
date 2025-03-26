using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderServiceController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderServiceController(OrderService orderService)
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
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(int userId)
    {
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return Ok(orders);
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateOrder(Order order)
    //{
    //    var createdOrder = await _orderService.CreateOrderAsync(order);
    //    return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
    //}

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateOrder(int id, Order order)
    //{
    //    if (id != order.Id)
    //    {
    //        return BadRequest();
    //    }

    //    await _orderService.UpdateOrderAsync(order);
    //    return NoContent();
    //}
}
