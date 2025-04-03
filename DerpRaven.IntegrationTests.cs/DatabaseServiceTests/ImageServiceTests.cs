using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using DerpRaven.Shared.Dtos;
using DerpRaven.Api;
using Microsoft.Extensions.Options;
namespace DerpRaven.IntegrationTests.cs.DatabaseServiceTests;

public class ImageServiceTests
{
    //private ImageService _imageService;
    //private AppDbContext _context;

    //[SetUp]
    //public void Setup()
    //{
    //    var options = new DbContextOptionsBuilder<AppDbContext>()
    //        .UseInMemoryDatabase(databaseName: "TestDatabase")
    //        .Options;

    //    _context = new AppDbContext(options);
    //    var logger = Substitute.For<ILogger<ImageService>>();
    //    IOptions<BlobStorageOptions> blobOptions = new();
    //    _imageService = new ImageService(blobOptions, _context, logger);

    //    _context.SaveChanges();
    //}

    //[TearDown]
    //public void TearDownAttribute()
    //{
    //    _context.Database.EnsureDeleted();
    //    _context.Dispose();
    //}
}
