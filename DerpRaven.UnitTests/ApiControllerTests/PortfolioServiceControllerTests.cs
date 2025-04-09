using DerpRaven.Api;
using DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
namespace DerpRaven.UnitTests.ApiControllerTests;

public class PortfolioServiceControllerTests
{
    PortfolioController _controller;

    [SetUp]
    public void Setup()
    {
        IPortfolioService portfolioService = Substitute.For<IPortfolioService>();
        IDerpRavenMetrics metrics = Substitute.For<IDerpRavenMetrics>();
        var dtoList = new List<PortfolioDto>()
        {
            new() { Description = "new portfolio 1", Id = 1, ImageIds = [1, 2, 3], Name = "my product", ProductTypeId = 1 },
            new() { Description = "new portfolio 2", Id = 2, ImageIds = [4, 5, 6], Name = "my product 2", ProductTypeId = 2 }
        };

        portfolioService.GetAllPortfoliosAsync().Returns(dtoList);
        portfolioService.GetPortfolioByIdAsync(1).Returns(dtoList[0]);
        portfolioService.CreatePortfolioAsync(Arg.Any<PortfolioDto>()).Returns(true);
        portfolioService.GetPortfoliosByTypeAsync("type1").Returns(dtoList.Where(c => c.ProductTypeId == 1).ToList());
        portfolioService.GetPortfoliosByNameAsync("my product").Returns(dtoList.Where(c => c.Name == "my product").ToList());
        portfolioService.DeletePortfolioAsync(1).Returns(true);
        portfolioService.UpdatePortfolioAsync(Arg.Any<PortfolioDto>()).Returns(true);

        _controller = new PortfolioController(portfolioService, metrics);

    }

    [Test]
    public async Task GetAllPortfolios()
    {
        // Act
        var result = await _controller.GetAllPortfolios() as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var portfolios = result.Value as List<PortfolioDto>;
        portfolios.ShouldNotBeEmpty();
        portfolios.Count.ShouldBe(2);
        portfolios.Any(c => c.Id == 1).ShouldBeTrue();
        portfolios.Any(c => c.Id == 2).ShouldBeTrue();
    }

    [Test]
    public async Task GetPortfolioById()
    {
        // Act
        var result = await _controller.GetPortfolioById(1) as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var portfolio = result.Value as PortfolioDto;
        portfolio.ShouldNotBeNull();
        portfolio.Id.ShouldBe(1);
    }

    [Test]
    public async Task GetPortfoliosByType()
    {
        // Act
        var result = await _controller.GetPortfoliosByType("type1") as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var portfolios = result.Value as List<PortfolioDto>;
        portfolios.ShouldNotBeEmpty();
        portfolios.Count.ShouldBe(1);
        portfolios.Any(c => c.ProductTypeId == 1).ShouldBeTrue();
    }

    [Test]
    public async Task GetPortfoliosByName()
    {
        // Act
        var result = await _controller.GetPortfoliosByName("my product") as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var portfolios = result.Value as List<PortfolioDto>;
        portfolios.ShouldNotBeEmpty();
        portfolios.Count.ShouldBe(1);
        portfolios.Any(c => c.Name == "my product").ShouldBeTrue();
    }

    [Test]
    public async Task CreatePortfolio()
    {
        // Arrange
        var portfolio = new PortfolioDto() { Description = "new portfolio 3", Id = 3, ImageIds = [7, 8, 9], Name = "my product 3", ProductTypeId = 3 };
        // Act
        var result = await _controller.CreatePortfolioAsync(portfolio) as CreatedResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status201Created);
    }

    [Test]
    public async Task UpdatePortfolio()
    {
        // Act
        var result = await _controller.UpdatePortfolio(1,
            new PortfolioDto()
            {
                Description = "new portfolio 1",
                Id = 1,
                ImageIds = [1, 2, 3],
                Name = "my product",
                ProductTypeId = 1
            }
            ) as NoContentResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }

    [Test]
    public async Task DeletePortfolio()
    {
        // Act
        var result = await _controller.DeletePortfolioAsync(1) as NoContentResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }
}
