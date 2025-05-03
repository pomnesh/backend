using Pomnesh.Application.Exceptions;
using Xunit;

namespace Pomnesh.UnitTests.Application.Exceptions;

public class ContextNotFoundErrorTests
{
    [Fact]
    public void Constructor_WithContextId_SetsPropertiesCorrectly()
    {
        // Arrange
        const long contextId = 456;

        // Act
        var exception = new ContextNotFoundError(contextId);

        // Assert
        Assert.Equal(contextId, exception.ContextId);
        Assert.Equal(404, exception.StatusCode);
        Assert.Equal($"The context with ID '456' was not found", exception.Description);
    }

    [Fact]
    public void StatusCode_Setter_ThrowsNotImplementedException()
    {
        // Arrange
        var exception = new ContextNotFoundError(456);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => exception.StatusCode = 500);
    }

    [Fact]
    public void Description_Setter_ThrowsNotImplementedException()
    {
        // Arrange
        var exception = new ContextNotFoundError(456);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => exception.Description = "New description");
    }

    [Fact]
    public void ContextId_IsReadOnly()
    {
        // Arrange
        const long contextId = 456;
        var exception = new ContextNotFoundError(contextId);

        // Act & Assert
        Assert.Equal(contextId, exception.ContextId);
        // Verify that ContextId is read-only by checking if it has a setter
        var property = typeof(ContextNotFoundError).GetProperty(nameof(ContextNotFoundError.ContextId));
        Assert.NotNull(property);
        Assert.Null(property.GetSetMethod());
    }
} 