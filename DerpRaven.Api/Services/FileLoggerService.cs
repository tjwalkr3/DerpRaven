using Microsoft.Extensions.Logging;

namespace DerpRaven.Api.Services;

/// <summary>
/// Example service that demonstrates subclass refactoring for testing.
/// This service has methods that interact with external systems (file system, clock)
/// which can be overridden in test subclasses.
/// </summary>
public class FileLoggerService
{
    private readonly ILogger<FileLoggerService> _logger;

    public FileLoggerService(ILogger<FileLoggerService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs a message to a file. This method calls virtual methods that can be overridden for testing.
    /// </summary>
    public async Task<bool> LogMessageAsync(string message)
    {
        try
        {
            var timestamp = GetCurrentTimestamp();
            var logEntry = $"[{timestamp}] {message}";
            var filePath = GetLogFilePath();
            
            await WriteToFileAsync(filePath, logEntry);
            _logger.LogInformation("Successfully logged message to {FilePath}", filePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log message");
            return false;
        }
    }

    /// <summary>
    /// Virtual method to get current timestamp - can be overridden in tests for predictable values.
    /// </summary>
    protected virtual DateTime GetCurrentTimestamp()
    {
        return DateTime.UtcNow;
    }

    /// <summary>
    /// Virtual method to get log file path - can be overridden in tests to avoid file system dependencies.
    /// </summary>
    protected virtual string GetLogFilePath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DerpRaven", "logs.txt");
    }

    /// <summary>
    /// Virtual method to write to file - can be overridden in tests to avoid actual file I/O.
    /// </summary>
    protected virtual async Task WriteToFileAsync(string filePath, string content)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.AppendAllTextAsync(filePath, content + Environment.NewLine);
    }

    /// <summary>
    /// Reads all log entries from the file.
    /// </summary>
    public async Task<List<string>> GetLogEntriesAsync()
    {
        try
        {
            var filePath = GetLogFilePath();
            if (!await FileExistsAsync(filePath))
            {
                return new List<string>();
            }

            var content = await ReadFileContentAsync(filePath);
            return content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read log entries");
            return new List<string>();
        }
    }

    /// <summary>
    /// Virtual method to check if file exists - can be overridden in tests.
    /// </summary>
    protected virtual async Task<bool> FileExistsAsync(string filePath)
    {
        return await Task.FromResult(File.Exists(filePath));
    }

    /// <summary>
    /// Virtual method to read file content - can be overridden in tests.
    /// </summary>
    protected virtual async Task<string> ReadFileContentAsync(string filePath)
    {
        return await File.ReadAllTextAsync(filePath);
    }
}