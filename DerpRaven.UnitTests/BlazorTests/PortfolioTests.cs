using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.Dtos;
using NSubstitute;
using Shouldly;

namespace DerpRaven.UnitTests;

public class PortfolioTests
{
    IBlazorPortfolioClient _portfolioClient;
    IBlazorImageClient _imageClient;

    [SetUp]
    public void Setup()
    {
        _portfolioClient = Substitute.For<IBlazorPortfolioClient>();
        _imageClient = Substitute.For<IBlazorImageClient>();
    }

    [Test]
    public void OnPortfolioChanged()
    {
        // Arrange
        var portfolios = new List<PortfolioDto>
        {
            new PortfolioDto { Id = 1, Name = "Portfolio 1", Description = "Description 1", ProductTypeId = 1, ImageIds = new List<int> { 1, 2 } },
            new PortfolioDto { Id = 2, Name = "Portfolio 2", Description = "Description 2", ProductTypeId = 2, ImageIds = new List<int> { 3, 4 } }
        };
        _portfolioClient.GetAllPortfoliosAsync().Returns(portfolios);

        // Act
        var portfolioId = 1;
        var product = portfolios.FirstOrDefault(p => p.Id == portfolioId);

        // Assert
        product.ShouldNotBeNull();
        product.Name.ShouldBe("Portfolio 1");
    }
}