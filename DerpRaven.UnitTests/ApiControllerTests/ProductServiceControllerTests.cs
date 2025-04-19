using DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
namespace DerpRaven.UnitTests.ApiControllerTests;

public class ProductServiceControllerTests
{
    ProductController _controller;
    [SetUp]
    public void Setup()
    {
        IProductService productService = Substitute.For<IProductService>();
        var dtoList = new List<ProductDto>()
        {
            new() { Description = "new product 1", Id = 1, ImageIds = [1, 2, 3], Name = "my product", ProductTypeId = 1 },
            new() { Description = "new product 2", Id = 2, ImageIds = [4, 5, 6], Name = "my product 2", ProductTypeId = 2 }
        };

        productService.GetAllProductsAsync().Returns(dtoList);
        productService.GetProductByIdAsync(1).Returns(dtoList[0]);
        productService.CreateProductAsync(Arg.Any<ProductDto>()).Returns(true);
        productService.UpdateProductAsync(Arg.Any<ProductDto>()).Returns(true);

        _controller = new ProductController(productService);
    }

    [Test]
    public async Task GetAllProducts()
    {
        // Act
        var result = await _controller.GetAllProductsAsync() as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var products = result.Value as List<ProductDto>;
        products.ShouldNotBeEmpty();
        products.Count.ShouldBe(2);
        products.Any(c => c.Id == 1).ShouldBeTrue();
        products.Any(c => c.Id == 2).ShouldBeTrue();
    }

    [Test]
    public async Task GetProductById()
    {
        // Act
        var result = await _controller.GetProductByIdAsync(1) as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var product = result.Value as ProductDto;
        product.ShouldNotBeNull();
        product.Id.ShouldBe(1);
    }





    [Test]
    public async Task CreateProduct()
    {
        // Arrange
        var product = new ProductDto { Description = "new product 1", Id = 1, ImageIds = [1, 2, 3], Name = "my product", ProductTypeId = 1 };
        // Act
        var result = await _controller.CreateProductAsync(product) as CreatedResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status201Created);
    }

    [Test]
    public async Task UpdateProduct()
    {
        // Act
        var result = await _controller.UpdateProduct(
            new ProductDto()
            {
                Description = "new product 1",
                Id = 1,
                ImageIds = [1, 2, 3],
                Name = "my product",
                ProductTypeId = 1
            }) as NoContentResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }
}
