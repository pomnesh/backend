using Pomnesh.Application.Exceptions;
using Xunit;

namespace Pomnesh.UnitTests.Application.Exceptions;

public class AttachmentNotFoundErrorTests
{
    [Fact]
    public void Constructor_WithAttachmentId_SetsPropertiesCorrectly()
    {
        // Arrange
        const long attachmentId = 123;

        // Act
        var exception = new AttachmentNotFoundError(attachmentId);

        // Assert
        Assert.Equal(attachmentId, exception.AttachmentId);
        Assert.Equal(404, exception.StatusCode);
        Assert.Equal($"The attachment with ID '123' was not found", exception.Description);
    }

    [Fact]
    public void StatusCode_Setter_ThrowsNotImplementedException()
    {
        // Arrange
        var exception = new AttachmentNotFoundError(123);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => exception.StatusCode = 500);
    }

    [Fact]
    public void Description_Setter_ThrowsNotImplementedException()
    {
        // Arrange
        var exception = new AttachmentNotFoundError(123);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => exception.Description = "New description");
    }
} 