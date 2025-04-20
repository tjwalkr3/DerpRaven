using DerpRaven.Api;
using DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
namespace DerpRaven.UnitTests.ApiControllerTests;

public class ImageControllerTests
{
    private ImageController _controller;

    [SetUp]
    public void Setup()
    {
        IImageService imageService = Substitute.For<IImageService>();
        IDerpRavenMetrics metrics = Substitute.For<IDerpRavenMetrics>();
        var dtoList = new List<ImageDto>()
        {
            new() { Id = 1, Alt = "alt text 1", Path = "file1.png" },
            new() { Id = 2, Alt = "alt text 2", Path = "file2.png" }
        };
        imageService.DeleteImageAsync(1).Returns(true);
        imageService.GetFileName(1).Returns("file1.png");
        imageService.GetImageAsync(1).Returns(new MemoryStream());
        imageService.ListImagesAsync().Returns(dtoList);
        imageService.UpdateImageDescriptionAsync(1, "new alt text").Returns(true);
        imageService.UploadImageAsync("file3.png", "alt text 3", new MemoryStream()).Returns(true);
        _controller = new ImageController(imageService, metrics);
    }

    [Test]
    public async Task DeleteImage()
    {
        // Act
        var result = await _controller.DeleteImage(1) as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
    }

    [Test]
    public async Task UploadImage_ShouldReturnOk()
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write("Hello World from a Fake File");
        writer.Flush();
        ms.Position = 0;

        var fileMock = new FormFile(ms, 0, ms.Length, "file3", "file3.png");
        var imageService = Substitute.For<IImageService>();
        imageService.UploadImageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>()).Returns(true);
        var controller = new ImageController(imageService, Substitute.For<IDerpRavenMetrics>());

        var result = await controller.UploadImage(fileMock, "alt text 3") as OkObjectResult;

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
    }

    [Test]
    public async Task ListImages()
    {
        // Act
        var result = await _controller.ListImages() as OkObjectResult;
        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var images = result.Value as List<ImageDto>;
        images.ShouldNotBeEmpty();
        images.Count.ShouldBe(2);
        images.Any(c => c.Id == 1).ShouldBeTrue();
        images.Any(c => c.Id == 2).ShouldBeTrue();
    }

    [Test]
    public async Task GetImage()
    {
        // Act
        var result = await _controller.GetImage(1) as FileStreamResult;
        // Assert
        result.ShouldNotBeNull();
        result.ContentType.ShouldBe("image/png");
        result.FileDownloadName.ShouldBe("file1.png");
    }
}