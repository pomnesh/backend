using Pomnesh.Application.Exceptions;
using Xunit;

namespace Pomnesh.UnitTests.Application.Exceptions;

public class RecollectionNotFoundErrorTests
{
    [Fact]
    public void Constructor_WithRecollectionId_SetsPropertiesCorrectly()
    {
        // Arrange
        const long recollectionId = 789;

        // Act
        var exception = new RecollectionNotFoundError(recollectionId);

        // Assert
        Assert.Equal(recollectionId, exception.RecollectionId);
        Assert.Equal(404, exception.StatusCode);
        Assert.Equal($"The recollection with ID '789' was not found", exception.Description);
    }

    [Fact]
    public void StatusCode_Setter_ThrowsNotImplementedException()
    {
        // Arrange
        var exception = new RecollectionNotFoundError(789);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => exception.StatusCode = 500);
    }

    [Fact]
    public void Description_Setter_ThrowsNotImplementedException()
    {
        // Arrange
        var exception = new RecollectionNotFoundError(789);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => exception.Description = "New description");
    }

    [Fact]
    public void RecollectionId_IsReadOnly()
    {
        // Arrange
        const long recollectionId = 789;
        var exception = new RecollectionNotFoundError(recollectionId);

        // Act & Assert
        Assert.Equal(recollectionId, exception.RecollectionId);
        // Verify that RecollectionId is read-only by checking if it has a setter
        var property = typeof(RecollectionNotFoundError).GetProperty(nameof(RecollectionNotFoundError.RecollectionId));
        Assert.NotNull(property);
        Assert.Null(property.GetSetMethod());
    }
} 