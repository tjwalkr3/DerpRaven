using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;

    public OrderController(IOrderService orderService, IUserService userService)
    {
        _orderService = orderService;
        _userService = userService;
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
        if (userId <= 0) return BadRequest("Invalid user ID");
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return Ok(orders);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetOrdersByUserEmail()
    {
        UserDto? user = await ControllerHelpers.GetCurrentUser(HttpContext.User, _userService);
        if (user == null) return Unauthorized();

        var requests = await _orderService.GetOrdersByUserEmailAsync(user.Email);
        return Ok(requests);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderDto order)
    {
        int createdOrderId = await _orderService.CreateOrderAsync(order);
        if (createdOrderId == 0) return BadRequest();
        return Created(string.Empty, new { OrderId = createdOrderId });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, string address, string email)
    {
        bool wasUpdated = await _orderService.UpdateOrderAsync(id, address, email);
        if (!wasUpdated) return BadRequest();
        return NoContent();
    }


}
