using DerpRaven.Blazor.ApiClients;
using DerpRaven.Blazor.Pages;
using DerpRaven.Shared.Dtos;
using NSubstitute;
using Shouldly;

namespace DerpRaven.UnitTests;

public class AddProductTests
{
    private AddProducts _addProducts;
    private IBlazorImageClient _imageClient;
    private IBlazorProductClient _productClient;

    [SetUp]
    public void Setup()
    {
        _imageClient = Substitute.For<IBlazorImageClient>();
        _productClient = Substitute.For<IBlazorProductClient>();
        _addProducts = new AddProducts(_imageClient, _productClient);
    }

    [Test]
    public void AddProduct()
    {
        // Arrange


        // Act


        // Assert
        throw new NotImplementedException();
    }

    [Test]
    public async Task LoadProducts()
    {
        // Arrange
        var products = new List<ProductDto> {
            new ProductDto { Id = 1, Name = "Product 1", Price = 10.0m, Quantity = 5, Description = "Description 1", ProductTypeId = 1 },
            new ProductDto { Id = 2, Name = "Product 2", Price = 20.0m, Quantity = 10, Description = "Description 2", ProductTypeId = 2 }
        };
        _productClient.GetAllProductsAsync().Returns(products);

        // Act
        await _addProducts.LoadProducts();

        // Assert
        _addProducts._products.Count.ShouldBe(2);
    }

    [Test]
    public void UpdateProduct()
    {
        // Arrange


        // Act


        // Assert
        throw new NotImplementedException();
    }
}
