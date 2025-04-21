using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
namespace DerpRaven.IntegrationTests.DatabaseServiceTests;

public class ProductServiceTests
{
    private ProductService _productService;
    private AppDbContext _context;
    private List<ImageEntity> images;
    private List<Order> orders;
    private ProductType type1;
    private ProductType type2;
    private User user1;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        var logger = Substitute.For<ILogger<ProductService>>();
        _productService = new ProductService(_context, logger);

        type1 = new() { Name = "Plushie" };
        type2 = new() { Name = "Art" };
        _context.ProductTypes.Add(type1);
        _context.ProductTypes.Add(type2);

        images = new()
        {
            new() { Alt = "an image", Path = "a random path", Products = [], Portfolios = []},
            new() { Alt = "an image 2", Path = "a random path 2", Products = [], Portfolios = []}
        };
        _context.Images.AddRange(images);

        orders = new()
        {
            new() { Address = "123 Street", Email = "test@example.com", OrderDate = DateTime.Now, User = user1, OrderedProducts = [] }
        };
        _context.Orders.AddRange(orders);

        user1 = new() { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Order(1)]
    [Test]
    public async Task CreateProduct()
    {
        // Arrange
        List<int> imageIds = images.Select(x => x.Id).ToList();
        var product = new ProductDto() { Id = 1, Name = "Test Product", Price = 100.0m, Quantity = 1, Description = "A description", ProductTypeId = type1.Id, ImageIds = imageIds };

        // Act
        await _productService.CreateProductAsync(product);
        var createdProduct = _context.Products.Find(1);

        // Assert
        createdProduct.ShouldNotBeNull();
        createdProduct.Name.ShouldBe("Test Product");
    }

    [Order(2)]
    [Test]
    public async Task GetAllProducts()
    {
        // Arrange
        List<Product> products = new()
        {
            new Product() { Name = "Test Product", Price = 100.0m, Quantity = 1, Description = "A description", ProductType = type1, Images = images },
            new Product() { Name = "Test Product2", Price = 150.0m, Quantity = 1, Description = "A description2", ProductType = type2, Images = images },
        };
        await _context.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        result.Any(p => p.Name == "Test Product").ShouldBeTrue();
        result.Any(p => p.Name == "Test Product2").ShouldBeTrue();
    }

    [Order(3)]
    [Test]
    public async Task GetProductById()
    {
        // Arrange
        var product1 = new Product() { Name = "Test Product", Price = 100.0m, Quantity = 1, Description = "A description", ProductType = type1, Images = images };
        await _context.Products.AddAsync(product1);
        await _context.SaveChangesAsync();

        // Act
        var product = await _productService.GetProductByIdAsync(1);

        // Assert
        product.ShouldNotBeNull();
        product.Name.ShouldBe("Test Product");
    }

    [Order(6)]
    [Test]
    public async Task UpdateProduct()
    {
        // Arrange
        var product1 = new Product { Id = 1, Name = "Test Product", Price = 100.0m, Quantity = 1, Description = "A description", ProductType = type1, Images = images };
        _context.Products.Add(product1);
        _context.SaveChanges();

        List<int> imageIds = images.Select(x => x.Id).ToList();
        var productDto = new ProductDto() { Id = 1, Name = "UpdatedName", Price = 150.0m, Quantity = 2, Description = "UpdatedDescription", ProductTypeId = type2.Id, ImageIds = imageIds };

        // Act
        await _productService.UpdateProductAsync(productDto);
        var updatedProduct = _context.Products.Find(1);

        // Assert
        updatedProduct.ShouldNotBeNull();
        updatedProduct.Name.ShouldBe("UpdatedName");
        updatedProduct.Price.ShouldBe(150.0m);
        updatedProduct.Quantity.ShouldBe(2);
        updatedProduct.Description.ShouldBe("UpdatedDescription");
        updatedProduct.ProductType.Id.ShouldBe(2);
    }
}
