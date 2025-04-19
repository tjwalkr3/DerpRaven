using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }


    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(ProductDto product)
    {
        var wasCreated = await _productService.CreateProductAsync(product);
        if (!wasCreated) return BadRequest();
        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductDto product)
    {
        bool wasUpdated = await _productService.UpdateProductAsync(product);
        if (!wasUpdated) return NotFound();
        return NoContent();
    }
}

