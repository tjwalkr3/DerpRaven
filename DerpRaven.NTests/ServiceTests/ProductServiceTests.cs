using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;

namespace DerpRaven.Tests.ServiceTests;

public class ProductServiceTests
{
    private DbContextOptions<AppDbContext> _dbContextOptions;
    private AppDbContext _dbContext;
    private ProductService _productService;
    private ILogger<ProductService> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<ProductService>>();
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _dbContext = new AppDbContext(_dbContextOptions);
        _productService = new ProductService(_dbContext, _logger);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Order(1)]
    [Test]
    public async Task CreateProduct()
    {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };
        var product = new Product { Id = 1, Name = "Test Product", ProductType = type1, Price = 100.0m, Quantity = 1, Description = "A description" };

        // Act
        await _productService.CreateProductAsync(product);
        var createdProduct = _dbContext.Products.Find(1);

        // Assert
        createdProduct.ShouldNotBeNull();
        createdProduct.Name.ShouldBe("Test Product");
    }

    [Order(2)]
    [Test]
    public async Task GetAllProducts()
    {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };
        var type2 = new ProductType { Id = 2, Name = "Art" };
        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m, Quantity = 1, Description = "A description" });
        _dbContext.Products.Add(new Product { Id = 2, Name = "Test Product 2", ProductType = type2, Price = 50.0m, Quantity = 1, Description = "A description" });
        _dbContext.SaveChanges();

        // Act
        var products = await _productService.GetAllProductsAsync();

        // Assert
        products.Where(s => true).Count().ShouldBe(2);
    }

    [Order(3)]
    [Test]
    public async Task GetProductById()
    {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };
        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m, Quantity = 1, Description = "A description" });
        _dbContext.SaveChanges();

        // Act
        var product = await _productService.GetProductByIdAsync(1);

        // Assert
        product.ShouldNotBeNull();
        product.Name.ShouldBe("Test Product 1");
    }

    [Order(4)]
    [Test]
    public async Task GetProductByName()
    {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };

        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m, Quantity = 1, Description = "A description" });
        _dbContext.SaveChanges();
        string productName = "Test Product 1";

        // Act
        var products = await _productService.GetProductsByNameAsync(productName);

        // Assert
        products.ShouldNotBeNull();
        products.First().Name.ShouldBe(productName);
    }

    [Order(5)]
    [Test]
    public async Task GetProductByType()
    {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };
        var type2 = new ProductType { Id = 2, Name = "Art" };

        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m, Quantity = 1, Description = "A description 1." });
        _dbContext.Products.Add(new Product { Id = 2, Name = "Test Product 2", ProductType = type2, Price = 100.0m, Quantity = 1, Description = "A description 2." });
        _dbContext.SaveChanges();

        // Act
        var products = await _productService.GetProductsByTypeAsync("Plushie");

        // Assert
        products.Where(p => true).Count().ShouldBe(1);
        products.First().Id.ShouldBe(1);
    }

    [Order(6)]
    [Test]
    public async Task UpdateProduct()
    {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };
        var product1 = new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m, Quantity = 1, Description = "A description" };
        _dbContext.Products.Add(product1);
        _dbContext.SaveChanges();

        // Act
        product1.Name = "Updated Product";
        await _productService.UpdateProductAsync(product1);
        var updatedProduct = _dbContext.Products.Find(1);

        // Assert
        updatedProduct.ShouldNotBeNull();
        updatedProduct.Name.ShouldBe("Updated Product");
    }
}
