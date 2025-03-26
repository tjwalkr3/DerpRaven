using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shouldly;
using NSubstitute;
using DerpRaven.Api.Dtos;
namespace DerpRaven.Tests.ServiceTests;

public class PortfolioServiceTests
{
    private OrderService _orderService;
    private AppDbContext _context;
    private List<Image> images;
    private ProductType type1;
    private ProductType type2;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        var logger = Substitute.For<ILogger<OrderService>>();
        _orderService = new OrderService(_context, logger);

        type1 = new() { Name = "Plushie" };
        type2 = new() { Name = "Art" };
        _context.ProductTypes.Add(type1);
        _context.ProductTypes.Add(type2);


        user1 = new() { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        user2 = new() { Id = 2, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer" };
        _context.Users.Add(user1);
        _context.Users.Add(user2);

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
