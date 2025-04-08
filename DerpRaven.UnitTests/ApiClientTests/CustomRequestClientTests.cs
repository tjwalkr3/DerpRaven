using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using NSubstitute;
using System.Net;
using Shouldly;
using System.Text.Json;
namespace DerpRaven.UnitTests.ApiClientTests;

public class CustomRequestClientTests
{
    IApiService _apiService;

    [SetUp]
    public void Setup()
    {
        string json = JsonSerializer.Serialize(new List<CustomRequestDto>
        {
            new CustomRequestDto
            {
                Id = 1,
                Description = "description1",
                Email = "email1",
                ProductTypeId = 1,
                Status = "Pending",
                UserId = 1
            }
        });
        _apiService = Substitute.For<IApiService>();
        _apiService.GetAsync(Arg.Any<string>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) });
        _apiService.PostAsJsonAsync(Arg.Any<string>(), Arg.Any<CustomRequestDto>()).Returns(new HttpResponseMessage(HttpStatusCode.OK));
        _apiService.PatchAsync(Arg.Any<string>(), Arg.Any<HttpContent>()).Returns(new HttpResponseMessage(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetAllCustomRequestsAsync()
    {
        // Arrange
        var client = new CustomRequestClient(_apiService);

        // Act
        var result = await client.GetAllCustomRequestsAsync();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<List<CustomRequestDto>>();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
        result[0].Description.ShouldBe("description1");
    }

    [Test]
    public async Task CreateCustomRequestAsync()
    {
        // Arrange
        var client = new CustomRequestClient(_apiService);
        var customRequest = new CustomRequestDto
        {
            Id = 1,
            Description = "description1",
            Email = "email1",
            ProductTypeId = 1,
            Status = "Pending",
            UserId = 1
        };

        // Act
        var result = await client.CreateCustomRequestAsync(customRequest);

        // Assert
        result.ShouldBeTrue();
    }

    [Test]
    public async Task GetCustomRequestByIdAsync()
    {
        // Arrange
        string json = JsonSerializer.Serialize(new CustomRequestDto
        {
            Id = 1,
            Description = "description1",
            Email = "email1",
            ProductTypeId = 1,
            Status = "Pending",
            UserId = 1
        });
        IApiService newApiService = Substitute.For<IApiService>();
        _apiService.GetAsync(Arg.Any<string>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) });
        var client = new CustomRequestClient(_apiService);

        // Act
        var result = await client.GetCustomRequestByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<CustomRequestDto>();
        result.Id.ShouldBe(1);
    }

    [Test]
    public async Task GetCustomRequestsByUserAsync()
    {
        // Arrange
        var client = new CustomRequestClient(_apiService);

        // Act
        var result = await client.GetCustomRequestsByUserEmailAsync();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<List<CustomRequestDto>>();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
    }

    [Test]
    public async Task GetCustomRequestsByStatusAsync()
    {
        // Arrange
        var client = new CustomRequestClient(_apiService);

        // Act
        var result = await client.GetCustomRequestsByStatusAsync("Pending");

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<List<CustomRequestDto>>();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
    }

    [Test]
    public async Task GetCustomRequestsByTypeAsync()
    {
        // Arrange
        var client = new CustomRequestClient(_apiService);

        // Act
        var result = await client.GetCustomRequestsByTypeAsync("type1");

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<List<CustomRequestDto>>();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
    }

    [Test]
    public async Task ChangeStatusAsync()
    {
        // Arrange
        var client = new CustomRequestClient(_apiService);

        // Act
        var result = await client.ChangeStatusAsync(1, "Approved");

        // Assert
        result.ShouldBeTrue();
    }
}
