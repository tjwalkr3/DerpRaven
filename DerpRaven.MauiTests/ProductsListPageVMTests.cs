using DerpRaven.Maui;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using NSubstitute;
using Shouldly;

namespace DerpRaven.MauiTests;

public class ProductsListPageVMTests
{
    private ProductsListPageViewModel? _viewModel;

    [SetUp]
    public void Setup()
    {
        var productClient = Substitute.For<IProductClient>();
        var imageClient = Substitute.For<IImageClient>();

        var viewModel = new ProductsListPageViewModel(productClient, imageClient);

    }

    [Test]
    public void PopulateProductViews()
    {
        // Arrange
        List<ProductDto> products = new List<ProductDto>
        {
            new ProductDto
            {
                Id = 1,
                ProductTypeId = 1,
                ImageIds = new List<int> { 1, 2 }
            },
            new ProductDto
            {
                Id = 2,
                ProductTypeId = 2,
                ImageIds = new List<int> { 3, 4 }
            }

        };

        List<ImageDto> images = new List<ImageDto>
        {
            new ImageDto { Id = 1, Path = "path1" },
            new ImageDto { Id = 2, Path = "path2" },
            new ImageDto { Id = 3, Path = "path3" },
            new ImageDto { Id = 4, Path = "path4" }
        };

        // Act
        _viewModel?.PopulateProductViews(products, images);

        // Assert
        _viewModel?.PlushieProducts.Count.ShouldBe(1);
        _viewModel?.ArtProducts.Count.ShouldBe(1);
        _viewModel?.PlushieProducts[0].Id.ShouldBe(1);
        _viewModel?.ArtProducts[0].Id.ShouldBe(3);
    }


}
