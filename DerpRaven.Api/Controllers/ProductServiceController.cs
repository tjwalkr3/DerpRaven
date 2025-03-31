using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductServiceController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductServiceController(IProductService productService)
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

    [HttpGet("type/{productType}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductsByTypeAsync(string productType)
    {
        var products = await _productService.GetProductsByTypeAsync(productType);
        return Ok(products);
    }

    [HttpGet("name/{name}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductsByNameAsync(string name)
    {
        var products = await _productService.GetProductsByNameAsync(name);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(ProductDto product)
    {
        var wasCreated = await _productService.CreateProductAsync(product);
        if (!wasCreated) return BadRequest();
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductDto product)
    {
        if (id != product.Id) return BadRequest();
        bool wasUpdated = await _productService.UpdateProductAsync(product);
        if (!wasUpdated) return NotFound();
        return NoContent();
    }
}

