# Subclass Refactoring for Testing Example

This directory contains an example of the **subclass refactoring** testing pattern, which is a technique for making code more testable by creating test-specific subclasses that override methods with external dependencies.

## What is Subclass Refactoring for Testing?

Subclass refactoring for testing is a refactoring technique where you:

1. Identify methods in a class that have external dependencies (file system, database, network, clock, etc.)
2. Make those methods `virtual` or `protected virtual` so they can be overridden
3. Create a test subclass that inherits from the original class
4. Override the problematic methods in the test subclass to provide predictable, fast, and isolated behavior

## Example Structure

### Original Service (`FileLoggerService.cs`)
```csharp
public class FileLoggerService
{
    // Main business logic method
    public async Task<bool> LogMessageAsync(string message)
    {
        var timestamp = GetCurrentTimestamp();        // ← External dependency (clock)
        var filePath = GetLogFilePath();              // ← External dependency (file system)
        await WriteToFileAsync(filePath, logEntry);   // ← External dependency (file I/O)
        return true;
    }

    // Virtual methods that can be overridden for testing
    protected virtual DateTime GetCurrentTimestamp() { ... }
    protected virtual string GetLogFilePath() { ... }
    protected virtual async Task WriteToFileAsync(...) { ... }
}
```

### Test Subclass (`TestableFileLoggerService.cs`)
```csharp
public class TestableFileLoggerService : FileLoggerService
{
    private readonly List<string> _inMemoryLogs = new();

    // Override external dependencies with test-friendly implementations
    protected override DateTime GetCurrentTimestamp() => _fixedTimestamp;
    protected override string GetLogFilePath() => "test-logs.txt";
    protected override async Task WriteToFileAsync(...) 
    {
        _inMemoryLogs.Add(content); // Store in memory instead of file
    }
}
```

### Tests (`FileLoggerServiceTests.cs`)
```csharp
[Test]
public async Task LogMessageAsync_ShouldLogWithTimestamp()
{
    // Arrange
    var testableService = new TestableFileLoggerService(_logger, fixedTime);
    
    // Act - Test the actual business logic
    var result = await testableService.LogMessageAsync("Test message");
    
    // Assert - Verify behavior without file system dependencies
    result.ShouldBeTrue();
    testableService.LogCount.ShouldBe(1);
}
```

## Benefits of This Pattern

1. **Fast Tests**: No actual file I/O or external system calls
2. **Predictable**: Fixed timestamps and controlled environment
3. **Isolated**: Tests don't interfere with each other or the file system
4. **Maintainable**: Business logic remains in the original class
5. **Observable**: Test subclass can expose internal state for verification

## When to Use This Pattern

Use subclass refactoring for testing when:

- You have a class with external dependencies (file system, network, clock, etc.)
- The dependencies make testing slow, unpredictable, or difficult
- You want to test the business logic without the external dependencies
- Dependency injection isn't practical or overkill for the situation

## Alternative Patterns

While subclass refactoring is useful, consider these alternatives:

1. **Dependency Injection**: Pass dependencies as constructor parameters (preferred for new code)
2. **Strategy Pattern**: Extract behavior into separate classes
3. **Wrapper/Adapter Pattern**: Wrap external dependencies in testable interfaces

## Running the Example

To see this pattern in action:

1. Build the solution
2. Run the tests in `FileLoggerServiceTests.cs`
3. Compare the fast, isolated test runs with what would happen if testing the original service directly

This example demonstrates how subclass refactoring can make legacy code more testable without major architectural changes.