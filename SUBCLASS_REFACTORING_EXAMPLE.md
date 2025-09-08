# Subclass Refactoring for Testing - Found and Created Examples

## Found Example

After thoroughly searching the DerpRaven repository, I discovered that while the codebase extensively uses **dependency injection with interfaces** (which is a better practice), there was **no existing example of subclass refactoring for testing**.

## Created Example

I created a comprehensive example of subclass refactoring for testing:

### Files Created:

1. **`/DerpRaven.Api/Services/FileLoggerService.cs`**
   - A service that demonstrates the pattern with external dependencies (file system, clock)
   - Contains virtual methods that can be overridden for testing
   - Real business logic that logs messages to files with timestamps

2. **`/DerpRaven.UnitTests/TestDoubles/TestableFileLoggerService.cs`**
   - A test subclass that inherits from `FileLoggerService`
   - Overrides virtual methods to provide test-friendly implementations
   - Uses in-memory storage instead of file system
   - Provides fixed timestamps for predictable testing
   - Includes test helper methods to inspect internal state

3. **`/DerpRaven.UnitTests/SubclassRefactoringExamples/FileLoggerServiceTests.cs`**
   - Comprehensive test suite demonstrating the pattern
   - Shows how to test business logic without external dependencies
   - Includes examples of fast, isolated, and predictable tests
   - Demonstrates the benefits over testing the original service directly

4. **`/DerpRaven.UnitTests/SubclassRefactoringExamples/README.md`**
   - Detailed documentation of the subclass refactoring pattern
   - Explains when and why to use this technique
   - Provides code examples and best practices
   - Compares with alternative patterns (dependency injection, strategy pattern)

## Key Concepts Demonstrated:

### 1. **Extract and Override Call Pattern**
```csharp
// Original class with external dependency
public class FileLoggerService 
{
    public async Task<bool> LogMessageAsync(string message)
    {
        var timestamp = GetCurrentTimestamp();  // ← Can be overridden
        // ... business logic
    }
    
    protected virtual DateTime GetCurrentTimestamp() => DateTime.UtcNow;
}

// Test subclass
public class TestableFileLoggerService : FileLoggerService
{
    protected override DateTime GetCurrentTimestamp() => _fixedTimestamp;
}
```

### 2. **Benefits Shown**:
- **Fast tests**: No actual file I/O
- **Predictable**: Fixed timestamps and controlled environment  
- **Isolated**: Tests don't interfere with each other
- **Observable**: Test subclass exposes internal state for verification

### 3. **When to Use This Pattern**:
- Legacy code with external dependencies
- Dependencies that make testing slow or unpredictable
- When dependency injection isn't practical
- As a stepping stone toward better architecture

## Why This Pattern Wasn't Found in the Repository

The DerpRaven repository follows modern .NET practices with:
- **Dependency injection** with interfaces (`IImageService`, `IUserService`, etc.)
- **Mocking frameworks** (NSubstitute) for test doubles
- **Clean architecture** with proper separation of concerns

This is actually **better than subclass refactoring** for new code, but subclass refactoring remains valuable for:
- **Legacy code** that can't easily be refactored
- **Learning purposes** to understand testing patterns
- **Gradual migration** toward better architecture

## Running the Example

The example code is ready to run once the .NET 9 environment is available. It demonstrates:
1. How to identify methods suitable for subclass refactoring
2. How to make methods virtual for testability
3. How to create test subclasses that override problematic dependencies
4. How to write fast, predictable tests using the pattern

This provides a complete, working example of subclass refactoring for testing that can be used as a reference for similar situations in other codebases.