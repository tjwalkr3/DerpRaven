using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
namespace DerpRaven.Tests.ServiceTests;

public class PortfolioServiceTests
{
    private PortfolioService _portfolioService;
    private AppDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        var logger = Substitute.For<ILogger<PortfolioService>>();
        _portfolioService = new PortfolioService(_context, logger);
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
        var type1 = new ProductType() { Id = 1, Name = "Plushie" };
        var portfolio = new Portfolio { Id = 1, Name = "Portfolio1", Description = "Description1", ProductType = type1 };

        // Act
        await _portfolioService.CreatePortfolioAsync(portfolio);
        var result = await _context.Portfolios.FindAsync(1);

        // Assert
        result.ShouldBe(portfolio);
    }

    [Order(1)]
    [Test]
    public async Task GetAllPortfolios()
    {
        // Arrange
        var type1 = new ProductType() { Id = 1, Name = "Plushie" };
        var type2 = new ProductType() { Id = 2, Name = "Art" };
        var portfolios = new List<Portfolio>
        {
            new Portfolio { Id = 1, Name = "Portfolio1", Description = "Description1", ProductType = type1 },
            new Portfolio { Id = 2, Name = "Portfolio2", Description = "Description2", ProductType = type2 }
        };
        await _context.Portfolios.AddRangeAsync(portfolios);
        await _context.SaveChangesAsync();

        // Act
        var result = await _portfolioService.GetAllPortfoliosAsync();

        // Assert
        result.ShouldBe(portfolios);
    }

    [Order(2)]
    [Test]
    public async Task GetPortfolioById()
    {
        // Arrange
        var type1 = new ProductType() { Id = 1, Name = "Plushie" };
        var portfolio = new Portfolio { Id = 1, Name = "Portfolio1", Description = "Description1", ProductType = type1 };
        await _context.Portfolios.AddAsync(portfolio);
        await _context.SaveChangesAsync();

        // Act
        var result = await _portfolioService.GetPortfolioByIdAsync(1);

        // Assert
        result.ShouldBe(portfolio);
    }

    [Order(3)]
    [Test]
    public async Task GetPortfoliosByType()
    {
        // Arrange
        var type1 = new ProductType() { Id = 1, Name = "Plushie" };
        var type2 = new ProductType() { Id = 2, Name = "Art" };
        var portfolios = new List<Portfolio>
        {
            new Portfolio { Id = 1, Name = "Portfolio1", Description = "Description1", ProductType = type1 },
            new Portfolio { Id = 2, Name = "Portfolio2", Description = "Description2", ProductType = type2 }
        };
        await _context.Portfolios.AddRangeAsync(portfolios);
        await _context.SaveChangesAsync();

        // Act
        var result = await _portfolioService.GetPortfoliosByTypeAsync(type1.Name);

        // Assert
        result.ShouldBe(portfolios.Where(p => p.ProductType.Name == type1.Name));
    }

    [Order(1)]
    [Test]
    public async Task UpdatePortfolio()
    {
        // Arrange
        var type1 = new ProductType() { Id = 1, Name = "Plushie" };
        var portfolio = new Portfolio { Id = 1, Name = "Portfolio1", Description = "Description1", ProductType = type1 };
        await _context.Portfolios.AddAsync(portfolio);
        await _context.SaveChangesAsync();

        portfolio.Name = "UpdatedPortfolio";
        portfolio.Description = "UpdatedDescription";

        // Act
        await _portfolioService.UpdatePortfolioAsync(portfolio);
        var result = await _context.Portfolios.FindAsync(1);

        // Assert
        result.ShouldBe(portfolio);
    }
}
