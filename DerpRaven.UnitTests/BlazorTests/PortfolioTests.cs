using DerpRaven.Api.Model;
using DerpRaven.Blazor.ApiClients;
using DerpRaven.Blazor.Pages;
using DerpRaven.Shared.Dtos;
using NSubstitute;
using Shouldly;

namespace DerpRaven.UnitTests;

public class PortfolioTests
{
    Portfolios _portfolios = default!;

    [SetUp]
    public void Setup()
    {
        IBlazorPortfolioClient _portfolioClient = Substitute.For<IBlazorPortfolioClient>();
        var listOfPortfolios = new List<PortfolioDto>
        {
            new PortfolioDto { Id = 1, Name = "Portfolio 1", Description = "Description 1", ProductTypeId = 1, ImageIds = new List<int> { 1, 2 } },
            new PortfolioDto { Id = 2, Name = "Portfolio 2", Description = "Description 2", ProductTypeId = 2, ImageIds = new List<int> { 3, 4 } }
        };
        _portfolioClient.GetAllPortfoliosAsync().Returns(listOfPortfolios);

        IBlazorImageClient _imageClient = Substitute.For<IBlazorImageClient>();
        Portfolios portfolios = new Portfolios(_imageClient, _portfolioClient);
    }


}