using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using DerpRaven.Shared.Dtos;
using DerpRaven.Api;
using Microsoft.Extensions.Options;
namespace DerpRaven.IntegrationTests.DatabaseServiceTests;

public class ImageServiceTests
{
    private ImageService _imageService;
    private AppDbContext _context;
    private List<ImageEntity> images;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new AppDbContext(options);

        var logger = Substitute.For<ILogger<ImageService>>();
        var blobService = Substitute.For<IBlobService>();
        blobService.DownloadAsync(Arg.Any<string>())
            .Returns(new MemoryStream([1, 2, 3, 4, 5]));

        _imageService = new ImageService(blobService, _context, logger);

        images = new()
        {
            new() { Alt = "an image1", Path = "a_random_path1.png", Products = [], Portfolios = []},
            new() { Alt = "an image2", Path = "a_random_path2.png", Products = [], Portfolios = []}
        };

        _context.Images.AddRange(images);
        _context.SaveChanges();
    }

    [TearDown]
    public void TearDownAttribute()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Order(1)]
    [Test]
    public async Task UploadImage()
    {
        // Arrange
        string fileName = "test_image.png";
        string altText = "Test Image";
        using var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });

        // Act
        var result = await _imageService.UploadImageAsync(fileName, altText, stream);

        // Assert
        result.ShouldBeTrue();
    }

    [Order(2)]
    [Test]
    public async Task GetFileName()
    {
        // Act
        var result = await _imageService.GetFileName(1);

        // Assert
        result.ShouldBe("a_random_path1.png");
    }

    [Order(3)]
    [Test]
    public async Task ListImages()
    {
        // Act
        var result = await _imageService.ListImagesAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result[0].Path.ShouldBe("a_random_path1.png");
        result[1].Path.ShouldBe("a_random_path2.png");
    }

    [Order(4)]
    [Test]
    public async Task GetImage()
    {
        // Act
        var result = await _imageService.GetImageAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Length.ShouldBe(5);
    }

    [Order(5)]
    [Test]
    public async Task GetImageNotFound()
    {
        // Act
        var result = await _imageService.GetImageAsync(999);

        // Assert
        result.ShouldBeNull();
    }

    [Order(6)]
    [Test]
    public async Task UploadImageWithNullStream()
    {
        // Arrange
        string fileName = "test_image.png";
        string altText = "Test Image";
        Stream stream = null!;

        // Act
        var result = await _imageService.UploadImageAsync(fileName, altText, stream);

        // Assert
        result.ShouldBeFalse();
    }


    [Order(7)]
    [Test]
    public async Task DeleteImageAsync()
    {
        // Act
        var result = await _imageService.DeleteImageAsync(1);

        // Assert
        result.ShouldBeTrue();
    }

    [Order(8)]
    [Test]
    public async Task DeleteImageNotFound()
    {
        // Act
        var result = await _imageService.DeleteImageAsync(999);
        // Assert
        result.ShouldBeFalse();
    }

    [Order(9)]
    [Test]
    public async Task UpdateImageDescriptionAsync()
    {
        // Act
        var result = await _imageService.UpdateImageDescriptionAsync(1, "Updated Description");

        // Assert
        result.ShouldBeTrue();
        var updatedImage = await _context.Images.FindAsync(1);
        updatedImage.ShouldNotBeNull();
        updatedImage.Alt.ShouldBe("Updated Description");
    }

    [Order(10)]
    [Test]
    public async Task UpdateImageDescriptionNotFound()
    {
        // Act
        var result = await _imageService.UpdateImageDescriptionAsync(999, "Updated Description");
        // Assert
        result.ShouldBeFalse();
    }

    [Order(11)]
    [Test]
    public async Task GetImageInfoAsync()
    {
        // Act
        var result = await _imageService.GetImageInfoAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
        result.Path.ShouldBe("a_random_path1.png");
        result.Alt.ShouldBe("an image1");
    }

    [Order(12)]
    [Test]
    public async Task GetImageInfoNotFound()
    {
        // Act
        var result = await _imageService.GetImageInfoAsync(999);

        // Assert
        result.ShouldBeNull();
    }

    [Test]
    public void MapToImageDto()
    {
        // Arrange
        var image = new ImageEntity
        {
            Id = 1,
            Alt = "alt text",
            Path = "file.png",
            Portfolios = [],
            Products = []
        };

        // Act
        var result = ImageService.MapToImageDto(image);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
        result.Alt.ShouldBe("alt text");
        result.Path.ShouldBe("file.png");
    }
}
