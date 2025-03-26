using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductServiceController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductServiceController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("type/{productType}")]
    public async Task<IActionResult> GetProductsByType(string productType)
    {
        var products = await _productService.GetProductsByTypeAsync(productType);
        return Ok(products);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetProductsByName(string name)
    {
        var products = await _productService.GetProductsByNameAsync(name);
        return Ok(products);
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateProduct(Product product)
    //{
    //    var createdProduct = await _productService.CreateProductAsync(product);
    //    return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    //}

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateProduct(int id, Product product)
    //{
    //    if (id != product.Id)
    //    {
    //        return BadRequest();
    //    }

    //    await _productService.UpdateProductAsync(product);
    //    return NoContent();
    //}
}

