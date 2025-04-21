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
        
        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Test]
    public async Task GetPortfolioImages()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public void RefreshPortfolioViewTest()
    {
        throw new NotImplementedException();
    }
}
