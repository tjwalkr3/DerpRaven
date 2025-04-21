using DerpRaven.Maui;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using Shouldly;
using NSubstitute;

namespace DerpRaven.MauiTests;

public class PortfolioPageViewModelTests
{
    private IPortfolioClient? _portfolioClient;
    private IImageHelpers? _imageHelpers;
    private PortfolioPageViewModel? _viewModel;

    [SetUp]
    public void Setup()
    {
        var portfolioClient = Substitute.For<IPortfolioClient>();
        var imageHelpers = Substitute.For<IImageHelpers>();

        var viewModel = new PortfolioPageViewModel(portfolioClient, imageHelpers);
    }

    [Test]
    public void PopulatePortfolioViewsTest()
    {
        // Arrange
        List<PortfolioDto> portfolios = new List<PortfolioDto>
        {
            new PortfolioDto
            {
                Id = 1,
                ProductTypeId = 1,
                ImageIds = new List<int> { 1, 2 }
            },
            new PortfolioDto
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
        _viewModel?.MakeCarouselView(portfolios, images);

        // Assert
        _viewModel?.PlushiePortfolios.Count.ShouldBe(1);
        _viewModel?.ArtPortfolios.Count.ShouldBe(1);
        _viewModel?.PlushiePortfolios[0].Portfolio.ImageIds[0].ShouldBe(1);
        _viewModel?.ArtPortfolios[0].Portfolio.ImageIds[0].ShouldBe(3);
    }

    [Test]
    public void MakeCarouselView()
    {
        // Arrange
        List<PortfolioDto> portfolios = new List<PortfolioDto>
        {
            new PortfolioDto
            {
                Id = 1,
                ProductTypeId = 1,
                ImageIds = new List<int> { 1, 2 }
            },
            new PortfolioDto
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
        //var CarouselViewModel = new CarouselViewModel(portfolios, images);

        // Assert
        _viewModel?.PlushiePortfolios.Count.ShouldBe(1);
        _viewModel?.ArtPortfolios.Count.ShouldBe(1);
    }

    [Test]
    public void GetPortfolioImages()
    {
        // Arrange
        List<PortfolioDto> portfolios = new List<PortfolioDto>
        {
            new PortfolioDto
            {
                Id = 1,
                ProductTypeId = 1,
                ImageIds = new List<int> { 1, 2 }
            },
            new PortfolioDto
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


        // Assert
        throw new NotImplementedException();
    }

    [Test]
    public void RefreshPortfolioViewTest()
    {
        // Arrange
        List<PortfolioDto> portfolios = new List<PortfolioDto>
        {
            new PortfolioDto
            {
                Id = 1,
                ProductTypeId = 1,
                ImageIds = new List<int> { 1, 2 }
            },
            new PortfolioDto
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

        // Assert
        _viewModel?.PlushiePortfolios.Count.ShouldBe(1);
        _viewModel?.ArtPortfolios.Count.ShouldBe(1);
    }
}
