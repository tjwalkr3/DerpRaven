using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using NSubstitute;
using System.Net;
using Shouldly;
namespace DerpRaven.UnitTests.ApiClientTests;

public class PortfolioClientTests
{
    [Test]
    public async Task GetAllPortfoliosAsync()
    {
        // Arrange
        IApiService _apiService = Substitute.For<IApiService>();
        var portfolioList = new List<PortfolioDto> { new PortfolioDto { Id = 1, Name = "Portfolio1", Description = "Description1" } };
        _apiService.GetFromJsonAsyncWithoutAuthorization<List<PortfolioDto>>(Arg.Any<string>()).Returns(portfolioList);
        var client = new PortfolioClient(_apiService);

        // Act
        var result = await client.GetAllPortfoliosAsync();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<List<PortfolioDto>>();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
        result[0].Name.ShouldBe("Portfolio1");
    }

    [Test]
    public async Task GetPortfolioByIdAsync()
    {
        // Arrange
        IApiService _apiService = Substitute.For<IApiService>();
        var portfolio = new PortfolioDto { Id = 1, Name = "Portfolio1", Description = "Description1" };
        _apiService.GetFromJsonAsyncWithoutAuthorization<PortfolioDto>(Arg.Any<string>()).Returns(portfolio);
        var client = new PortfolioClient(_apiService);

        // Act
        var result = await client.GetPortfolioByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<PortfolioDto>();
        result.Id.ShouldBe(1);
        result.Name.ShouldBe("Portfolio1");
    }

    [Test]
    public async Task GetPortfoliosByTypeAsync()
    {
        // Arrange
        IApiService _apiService = Substitute.For<IApiService>();
        var portfolioList = new List<PortfolioDto> { new PortfolioDto { Id = 1, Name = "Portfolio1", Description = "Description1" } };
        _apiService.GetFromJsonAsyncWithoutAuthorization<List<PortfolioDto>>(Arg.Any<string>()).Returns(portfolioList);
        var client = new PortfolioClient(_apiService);

        // Act
        var result = await client.GetPortfoliosByTypeAsync("Type1");

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<List<PortfolioDto>>();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
        result[0].Name.ShouldBe("Portfolio1");
    }

    [Test]
    public async Task GetPortfoliosByNameAsync()
    {
        // Arrange
        IApiService _apiService = Substitute.For<IApiService>();
        var portfolioList = new List<PortfolioDto> { new PortfolioDto { Id = 1, Name = "Portfolio1", Description = "Description1" } };
        _apiService.GetFromJsonAsyncWithoutAuthorization<List<PortfolioDto>>(Arg.Any<string>()).Returns(portfolioList);
        var client = new PortfolioClient(_apiService);

        // Act
        var result = await client.GetPortfoliosByNameAsync("Portfolio1");

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<List<PortfolioDto>>();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
        result[0].Name.ShouldBe("Portfolio1");
    }

    [Test]
    public async Task CreatePortfolioAsync()
    {
        // Arrange
        IApiService _apiService = Substitute.For<IApiService>();
        var portfolio = new PortfolioDto { Id = 1, Name = "Portfolio1", Description = "Description1" };
        _apiService.PostAsJsonAsync(Arg.Any<string>(), Arg.Any<PortfolioDto>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK));
        var client = new PortfolioClient(_apiService);

        // Act
        var result = await client.CreatePortfolioAsync(portfolio);

        // Assert
        result.ShouldBeTrue();
    }

    [Test]
    public async Task UpdatePortfolioAsync()
    {
        // Arrange
        IApiService _apiService = Substitute.For<IApiService>();
        var portfolio = new PortfolioDto { Id = 1, Name = "Portfolio1", Description = "Description1" };
        _apiService.PutAsJsonAsync<PortfolioDto>(Arg.Any<string>(), Arg.Any<PortfolioDto>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK));
        var client = new PortfolioClient(_apiService);

        // Act
        var result = await client.UpdatePortfolioAsync(1, portfolio);

        // Assert
        result.ShouldBeTrue();
    }

    [Test]
    public async Task DeletePortfolioAsync()
    {
        // Arrange
        IApiService _apiService = Substitute.For<IApiService>();
        _apiService.DeleteAsync(Arg.Any<string>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK));
        var client = new PortfolioClient(_apiService);

        // Act
        var result = await client.DeletePortfolioAsync(1);

        // Assert
        result.ShouldBeTrue();
    }
}
