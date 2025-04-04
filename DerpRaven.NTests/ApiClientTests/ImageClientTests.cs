using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using NSubstitute;
using System.Net;
using Shouldly;
namespace DerpRaven.NTests.ApiClientTests;

public class ImageClientTests
{

    [Test]
    public async Task UploadImageAsync()
    {
        // Arrange
        IApiService _apiService = Substitute.For<IApiService>();
        _apiService.PostAsync(Arg.Any<string>(), Arg.Any<HttpContent>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK));
        var client = new ImageClient(_apiService);
        var file = Substitute.For<IBrowserFile>();
        file.Name.Returns("test.png");
        file.OpenReadStream(Arg.Any<long>()).Returns(new MemoryStream());

        // Act
        bool result = await client.UploadImageAsync(file, "Test description");

        // Assert
        result.ShouldBeTrue();
    }

    [Test]
    public async Task ListImagesAsync()
    {
        // Arrange
        IApiService apiService = Substitute.For<IApiService>();
        var imageList = new List<ImageDto> { new ImageDto { Id = 1, Alt = "alt1", Path = "path1" } };
        apiService.GetFromJsonAsyncWithoutAuthorization<List<ImageDto>>(Arg.Any<string>()).Returns(imageList);
        var client = new ImageClient(apiService);

        // Act
        var result = await client.ListImagesAsync();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<List<ImageDto>>();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
        result[0].Alt.ShouldBe("alt1");
    }

    [Test]
    public async Task GetImageAsync_ShouldBeNull()
    {
        // Arrange
        IApiService apiService = Substitute.For<IApiService>();
        apiService.GetAsyncWithoutAuthorization(Arg.Any<string>())
            .Returns(new HttpResponseMessage(HttpStatusCode.Unauthorized));
        var client = new ImageClient(apiService);

        // Act
        var result = await client.GetImageAsync(1);

        // Assert
        result.ShouldBeNull();
    }

    [Test]
    public async Task GetImageAsync_ShouldReturnByteArray()
    {
        // Arrange
        var imageBytes = new byte[] { 1, 2, 3, 4, 5 };
        IApiService apiService = Substitute.For<IApiService>();
        apiService.GetAsyncWithoutAuthorization(Arg.Any<string>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(imageBytes) });
        var client = new ImageClient(apiService);

        // Act
        var result = await client.GetImageAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<byte[]>();
        result.Length.ShouldBe(imageBytes.Length);
    }

    [Test]
    public async Task DeleteImageAsync_ShouldBeFalse()
    {
        // Arrange
        IApiService apiService = Substitute.For<IApiService>();
        apiService.DeleteAsync(Arg.Any<string>())
            .Returns(new HttpResponseMessage(HttpStatusCode.Unauthorized));
        var client = new ImageClient(apiService);

        // Act
        var result = await client.DeleteImageAsync(1);

        // Assert
        result.ShouldBeFalse();
    }

    [Test]
    public async Task DeleteImageAsync()
    {
        // Arrange
        IApiService apiService = Substitute.For<IApiService>();
        apiService.DeleteAsync(Arg.Any<string>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK));
        var client = new ImageClient(apiService);

        // Act
        var result = await client.DeleteImageAsync(1);

        // Assert
        result.ShouldBeTrue();
    }

    [Test]
    public async Task GetImageInfoAsync_ShouldBeNull()
    {
        // Arrange
        IApiService apiService = Substitute.For<IApiService>();
        apiService.GetFromJsonAsyncWithoutAuthorization<ImageDto>(Arg.Any<string>())
            .Returns((ImageDto?)null);
        var client = new ImageClient(apiService);

        // Act
        var result = await client.GetImageInfoAsync(1);

        // Assert
        result.ShouldBeNull();
    }

    [Test]
    public async Task GetImageInfoAsync()
    {
        // Arrange
        var imageDto = new ImageDto { Id = 1, Alt = "alt1", Path = "path1" };
        IApiService apiService = Substitute.For<IApiService>();
        apiService.GetFromJsonAsyncWithoutAuthorization<ImageDto>(Arg.Any<string>())
            .Returns(imageDto);
        var client = new ImageClient(apiService);

        // Act
        var result = await client.GetImageInfoAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<ImageDto>();
        result.Id.ShouldBe(1);
        result.Alt.ShouldBe("alt1");
    }
}
