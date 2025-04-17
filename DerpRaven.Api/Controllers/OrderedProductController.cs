using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace DerpRaven.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class OrderedProductController : ControllerBase
{
    private readonly IOrderedProductService _orderedProductService;

    public OrderedProductController(IOrderedProductService orderedProductService)
    {
        _orderedProductService = orderedProductService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderedProductsByOrderId(int id)
    {
        if (id <= 0) return BadRequest("Invalid order ID");
        var orderedProducts = await _orderedProductService.GetOrderedProductsByOrderId(id);
        return Ok(orderedProducts);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderedProducts(List<OrderedProductDto> orderedProducts)
    {
        if (orderedProducts == null || orderedProducts.Count == 0) return BadRequest("No ordered products provided");
        bool completed = await _orderedProductService.CreateOrderedProducts(orderedProducts);
        if (!completed) return BadRequest("Failed to create ordered products");
        return Created();
    }
}
