using DerpRaven.Api.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DerpRaven.UnitTests.TestDoubles;

/// <summary>
/// Example of subclass refactoring for testing.
/// This class inherits from FileLoggerService and overrides virtual methods 
/// to provide test-friendly implementations without file system dependencies.
/// </summary>
public class TestableFileLoggerService : FileLoggerService
{
    private readonly List<string> _inMemoryLogs = new();
    private readonly DateTime _fixedTimestamp;

    public TestableFileLoggerService(ILogger<FileLoggerService> logger, DateTime? fixedTimestamp = null) 
        : base(logger)
    {
        _fixedTimestamp = fixedTimestamp ?? new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    }

    /// <summary>
    /// Override to return a fixed timestamp for predictable testing.
    /// </summary>
    protected override DateTime GetCurrentTimestamp()
    {
        return _fixedTimestamp;
    }

    /// <summary>
    /// Override to return a test file path instead of actual file system path.
    /// </summary>
    protected override string GetLogFilePath()
    {
        return "test-logs.txt";
    }

    /// <summary>
    /// Override to write to in-memory storage instead of actual file system.
    /// </summary>
    protected override async Task WriteToFileAsync(string filePath, string content)
    {
        _inMemoryLogs.Add(content);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Override to check in-memory storage instead of actual file system.
    /// </summary>
    protected override async Task<bool> FileExistsAsync(string filePath)
    {
        return await Task.FromResult(_inMemoryLogs.Count > 0);
    }

    /// <summary>
    /// Override to read from in-memory storage instead of actual file system.
    /// </summary>
    protected override async Task<string> ReadFileContentAsync(string filePath)
    {
        return await Task.FromResult(string.Join(Environment.NewLine, _inMemoryLogs));
    }

    // Test helper methods to inspect internal state
    public List<string> GetInMemoryLogs() => new(_inMemoryLogs);
    public void ClearLogs() => _inMemoryLogs.Clear();
    public int LogCount => _inMemoryLogs.Count;
}