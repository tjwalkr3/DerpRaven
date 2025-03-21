using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DerpRaven.Tests.ServiceTests;

public class CustomRequestServiceTests
{
    private CustomRequestService _customRequestService;
    private AppDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        var logger = Substitute.For<ILogger<CustomRequestService>>();
        _customRequestService = new CustomRequestService(_context, logger);

        _context.ProductTypes.Add(new() { Name = "Plushie" });
        _context.ProductTypes.Add(new() { Name = "Art" });
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Order(1)]
    [Test]
    public async Task CreateCustomRequest()
    {
        var customRequest = new CustomRequest { Id = 1, Status = "Pending" };

        await _customRequestService.CreateCustomRequest(customRequest);
        var result = await _context.CustomRequests.FindAsync(1);

        result.ShouldBe(customRequest);
    }

    [Order(2)]
    [Test]
    public async Task GetAllCustomRequests()
    {
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, Status = "Pending" },
            new CustomRequest { Id = 2, Status = "Completed" }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        var result = await _customRequestService.GetAllCustomRequestsAsync();

        result.ShouldBe(customRequests);
    }

    [Order(3)]
    [Test]
    public async Task GetCustomRequestById()
    {
        var customRequest = new CustomRequest { Id = 1, Status = "Pending" };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        var result = await _customRequestService.GetCustomRequestById(1);

        result.ShouldBe(customRequest);
    }

    [Order(4)]
    [Test]
    public async Task GetCustomRequestByUser()
    {
        User user1 = new User { Id = 1, Name = "User1", OAuth = "OAuth1", Email = "user1@example.com", Active = true, Role = "customer" };
        User user2 = new User { Id = 1, Name = "User2", OAuth = "OAuth2", Email = "user2@example.com", Active = true, Role = "customer" };
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, Status = "Pending", User = user1 },
            new CustomRequest { Id = 2, Status = "Completed", User = user2 }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        var result = await _customRequestService.GetCustomRequestsByUserAsync(1);

        result.ShouldBe(customRequests);
    }

    [Order(5)]
    [Test]
    public async Task GetCustomRequestByStatus()
    {
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, Status = "Pending" },
            new CustomRequest { Id = 2, Status = "Pending" }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        var result = await _customRequestService.GetCustomRequestsByStatusAsync("Pending");

        result.ShouldBe(customRequests);
    }

    [Order(6)]
    [Test]
    public async Task GetCustomRequestByType()
    {
        var customRequests = new List<CustomRequest>
        {
            new CustomRequest { Id = 1, ProductType = "Type1" },
            new CustomRequest { Id = 2, ProductType = "Type1" }
        };
        await _context.CustomRequests.AddRangeAsync(customRequests);
        await _context.SaveChangesAsync();

        var result = await _customRequestService.GetCustomRequestsByTypeAsync("Type1");

        result.ShouldBe(customRequests);
    }

    [Order(1)]
    [Test]
    public async Task UpdateCustomRequest()
    {
        var customRequest = new CustomRequest { Id = 1, Status = "Pending", Email = "test@example.com", Address = "123 Street" };
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();

        customRequest.Status = "Completed";
        customRequest.Email = "updated@example.com";
        customRequest.Address = "456 Avenue";

        await _customRequestService.UpdateCustomRequest(customRequest);
        var result = await _context.CustomRequests.FindAsync(1);

        result.ShouldBe(customRequest);
    }
}
