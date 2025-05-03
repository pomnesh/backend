using Pomnesh.Application.Models;
using Pomnesh.Domain.Enum;
using Xunit;
namespace Pomnesh.UnitTests.Application.Models;

public class AttachmentCreateRequestTests
{
    [Fact]
    public void Constructor_Default_InitializesPropertiesWithDefaultValues()
    {
        // Arrange & Act
        var request = new AttachmentCreateRequest();

        // Assert
        Assert.Equal(default(AttachmentType), request.Type);
        Assert.Equal(0, request.FileId);
        Assert.Equal(0, request.OwnerId);
        Assert.Null(request.OriginalLink);
        Assert.Equal(0, request.ContextId);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var request = new AttachmentCreateRequest
        {
            Type = AttachmentType.Photo,
            FileId = 123,
            OwnerId = 1,
            OriginalLink = "https://example.com/image.jpg",
            ContextId = 1
        };

        // Act & Assert
        Assert.Equal(AttachmentType.Photo, request.Type);
        Assert.Equal(123, request.FileId);
        Assert.Equal(1, request.OwnerId);
        Assert.Equal("https://example.com/image.jpg", request.OriginalLink);
        Assert.Equal(1, request.ContextId);
    }

    [Fact]
    public void Properties_CanBeModified()
    {
        // Arrange
        var request = new AttachmentCreateRequest
        {
            Type = AttachmentType.Photo,
            FileId = 123
        };

        // Act
        request.Type = AttachmentType.Photo;
        request.FileId = 456;
        request.OwnerId = 2;
        request.OriginalLink = "https://example.com/new_image.jpg";
        request.ContextId = 2;

        // Assert
        Assert.Equal(AttachmentType.Photo, request.Type);
        Assert.Equal(456, request.FileId);
        Assert.Equal(2, request.OwnerId);
        Assert.Equal("https://example.com/new_image.jpg", request.OriginalLink);
        Assert.Equal(2, request.ContextId);
    }

    [Fact]
    public void OriginalLink_CanBeSetToNull()
    {
        // Arrange
        var request = new AttachmentCreateRequest
        {
            OriginalLink = "https://example.com"
        };

        // Act
        request.OriginalLink = null;

        // Assert
        Assert.Null(request.OriginalLink);
    }
} 