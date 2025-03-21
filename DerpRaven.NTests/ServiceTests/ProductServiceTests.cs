using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;

namespace DerpRaven.Tests.ServiceTests;

public class ProductServiceTests {
    private DbContextOptions<AppDbContext> _dbContextOptions;
    private AppDbContext _dbContext;
    private ProductService _productService;
    private ILogger<ProductService> _logger;

    [SetUp]
    public void Setup() {
        _logger = Substitute.For<ILogger<ProductService>>();
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _dbContext = new AppDbContext(_dbContextOptions);
        _productService = new ProductService(_dbContext, _logger);
    }

    [TearDown]
    public void TearDown() {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Order(1)]
    [Test]
    public async Task CreateProduct() {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };
        var product = new Product { Id = 1, Name = "Test Product", ProductType = type1, Price = 100.0m };

        // Act
        await _productService.CreateProductAsync(product);
        var createdProduct = _dbContext.Products.Find(1);

        // Assert
        createdProduct.ShouldNotBeNull();
        createdProduct.Name.ShouldBe("Test Product");
    }

    [Order(2)]
    [Test]
    public async Task GetAllProducts() {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };
        var type2 = new ProductType { Id = 1, Name = "Art" };
        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m });
        _dbContext.Products.Add(new Product { Id = 2, Name = "Test Product 2", ProductType = type2, Price = 50.0m });
        _dbContext.SaveChanges();

        // Act
        var products = await _productService.GetAllProductsAsync();

        // Assert
        products.Where(s => true).Count().ShouldBe(2);
    }

    [Order(3)]
    [Test]
    public void GetProductById() {
        // Arrange
        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m });
        _dbContext.SaveChanges();

        // Act
        var product = _productService.GetProductById(1);

        // Assert
        product.ShouldNotBeNull();
        product.Name.ShouldBe("Test Product");
    }

    [Order(4)]
    [Test]
    public void GetProductByName() {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };

        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m });
        _dbContext.SaveChanges();

        // Act
        var product = _productService.GetProductByName("Test Product");

        // Assert
        product.ShouldNotBeNull();
        product.Name.ShouldBe("Test Product");
    }

    [Order(5)]
    [Test]
    public void GetProductByType() {
        // Arrange
        var type1 = new ProductType { Id = 1, Name = "Plushie" };

        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m });
        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m });
        _dbContext.SaveChanges();

        // Act
        var products = _productService.GetProductsByType("Electronics");

        // Assert
        products.Count().ShouldBe(2);
    }

    [Order(6)]
    [Test]
    public void UpdateProduct() {
        // Arrange
        _dbContext.Products.Add(new Product { Id = 1, Name = "Test Product 1", ProductType = type1, Price = 100.0m });
        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();

        // Act
        product.Name = "Updated Product";
        _productService.UpdateProduct(product);
        var updatedProduct = _dbContext.Products.Find(1);

        // Assert
        updatedProduct.ShouldNotBeNull();
        updatedProduct.Name.ShouldBe("Updated Product");
    }
}
