using DerpRaven.Api.Services;
using DerpRaven.UnitTests.TestDoubles;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace DerpRaven.UnitTests.SubclassRefactoringExamples;

/// <summary>
/// Example of testing using subclass refactoring.
/// This demonstrates how to test a class by inheriting from it and overriding
/// methods that have external dependencies (file system, clock, etc.)
/// </summary>
public class FileLoggerServiceTests
{
    private TestableFileLoggerService _testableService;
    private ILogger<FileLoggerService> _logger;
    private readonly DateTime _fixedTestTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<FileLoggerService>>();
        _testableService = new TestableFileLoggerService(_logger, _fixedTestTime);
    }

    [Test]
    public async Task LogMessageAsync_ShouldLogWithTimestamp()
    {
        // Arrange
        var message = "Test log message";
        var expectedLogEntry = $"[{_fixedTestTime:yyyy-MM-dd HH:mm:ss}] {message}";

        // Act
        var result = await _testableService.LogMessageAsync(message);

        // Assert
        result.ShouldBeTrue();
        _testableService.LogCount.ShouldBe(1);
        _testableService.GetInMemoryLogs()[0].ShouldBe(expectedLogEntry);
    }

    [Test]
    public async Task LogMessageAsync_MultipleCalls_ShouldAccumulateLogs()
    {
        // Arrange
        var messages = new[] { "First message", "Second message", "Third message" };

        // Act
        foreach (var message in messages)
        {
            await _testableService.LogMessageAsync(message);
        }

        // Assert
        _testableService.LogCount.ShouldBe(3);
        var logs = _testableService.GetInMemoryLogs();
        logs[0].ShouldContain("First message");
        logs[1].ShouldContain("Second message");
        logs[2].ShouldContain("Third message");
    }

    [Test]
    public async Task GetLogEntriesAsync_ShouldReturnAllLoggedMessages()
    {
        // Arrange
        await _testableService.LogMessageAsync("Message 1");
        await _testableService.LogMessageAsync("Message 2");

        // Act
        var logEntries = await _testableService.GetLogEntriesAsync();

        // Assert
        logEntries.Count.ShouldBe(2);
        logEntries[0].ShouldContain("Message 1");
        logEntries[1].ShouldContain("Message 2");
    }

    [Test]
    public async Task GetLogEntriesAsync_WhenNoLogs_ShouldReturnEmptyList()
    {
        // Act
        var logEntries = await _testableService.GetLogEntriesAsync();

        // Assert
        logEntries.ShouldBeEmpty();
    }

    /// <summary>
    /// This test demonstrates testing the original service directly 
    /// (without subclass refactoring) for comparison.
    /// Note: This would actually create files on the file system!
    /// </summary>
    [Test]
    [Ignore("This test would create actual files - use only for demonstration")]
    public async Task OriginalService_WouldRequireFileSystemCleanup()
    {
        // Arrange
        var originalService = new FileLoggerService(_logger);

        // Act
        var result = await originalService.LogMessageAsync("This would create an actual file");

        // Assert
        result.ShouldBeTrue();
        // Note: We'd need to clean up the actual file after this test
    }

    [TearDown]
    public void TearDown()
    {
        _testableService.ClearLogs();
    }
}