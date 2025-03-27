using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shouldly;
using NSubstitute;
using DerpRaven.Shared.Dtos;
namespace DerpRaven.Tests.ServiceTests;

public class PortfolioServiceTests
{
    private PortfolioService _portfolioService;
    private AppDbContext _context;
    private List<ImageEntity> images;
    private ProductType type1;
    private ProductType type2;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        var logger = Substitute.For<ILogger<PortfolioService>>();
        _portfolioService = new PortfolioService(_context, logger);

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
    public async Task CreatePortfolio()
    {
        // Arrange
        List<int> imageIds = images.Select(x => x.Id).ToList();
        var portfolioDto = new PortfolioDto { Name = "Portfolio1", Description = "Description1", ProductTypeId = type1.Id, ImageIds = imageIds };

        // Act
        await _portfolioService.CreatePortfolioAsync(portfolioDto);
        var result = await _context.Portfolios.FindAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Portfolio1");
    }

    [Order(1)]
    [Test]
    public async Task GetAllPortfolios()
    {
        // Arrange
        var portfolios = new List<Portfolio>
        {
            new Portfolio { Name = "Portfolio1", Description = "Description1", ProductType = type1, Images = images },
            new Portfolio { Name = "Portfolio2", Description = "Description2", ProductType = type2, Images = images }
        };
        await _context.Portfolios.AddRangeAsync(portfolios);
        await _context.SaveChangesAsync();

        // Act
        var result = await _portfolioService.GetAllPortfoliosAsync();

        // Assert
        result.Any(p => p.Name == "Portfolio1").ShouldBeTrue();
        result.Any(p => p.Name == "Portfolio2").ShouldBeTrue();
    }

    [Order(2)]
    [Test]
    public async Task GetPortfolioById()
    {
        // Arrange
        var portfolio = new Portfolio { Name = "Portfolio1", Description = "Description1", ProductType = type1, Images = images };
        await _context.Portfolios.AddAsync(portfolio);
        await _context.SaveChangesAsync();

        // Act
        var result = await _portfolioService.GetPortfolioByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Portfolio1");
    }

    [Order(3)]
    [Test]
    public async Task GetPortfoliosByType()
    {
        // Arrange
        var portfolios = new List<Portfolio>
        {
            new Portfolio { Name = "Portfolio1", Description = "Description1", ProductType = type1, Images = images },
            new Portfolio { Name = "Portfolio2", Description = "Description2", ProductType = type2, Images = images }
        };
        await _context.Portfolios.AddRangeAsync(portfolios);
        await _context.SaveChangesAsync();

        // Act
        var result = await _portfolioService.GetPortfoliosByTypeAsync(type1.Name);

        // Assert
        result.Single().Name.ShouldBe("Portfolio1");
    }

    [Order(1)]
    [Test]
    public async Task UpdatePortfolio()
    {
        // Arrange
        var portfolio = new Portfolio { Name = "Portfolio1", Description = "Description1", ProductType = type1, Images = images };
        await _context.Portfolios.AddAsync(portfolio);
        await _context.SaveChangesAsync();

        List<int> imageIds = images.Select(x => x.Id).ToList();
        var portfolioDto = new PortfolioDto { Id = 1, Name = "UpdatedPortfolio", Description = "UpdatedDescription", ProductTypeId = type1.Id, ImageIds = imageIds };

        // Act
        await _portfolioService.UpdatePortfolioAsync(portfolioDto);
        var result = await _context.Portfolios.FindAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("UpdatedPortfolio");
        result.Description.ShouldBe("UpdatedDescription");
    }
}
