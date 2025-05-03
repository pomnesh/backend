using Pomnesh.Application.Models;
using Xunit;

namespace Pomnesh.UnitTests.Application.Models;

public class ChatContextCreateRequestTests
{
    [Fact]
    public void Constructor_Default_InitializesPropertiesWithDefaultValues()
    {
        // Arrange & Act
        var request = new ChatContextCreateRequest();

        // Assert
        Assert.Equal(0, request.MessageId);
        Assert.Null(request.MessageText);
        Assert.Equal(default(DateTime), request.MessageDate);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var testDate = new DateTime(2024, 3, 15, 14, 30, 0);
        var request = new ChatContextCreateRequest
        {
            MessageId = 123,
            MessageText = "Test message",
            MessageDate = testDate
        };

        // Act & Assert
        Assert.Equal(123, request.MessageId);
        Assert.Equal("Test message", request.MessageText);
        Assert.Equal(testDate, request.MessageDate);
    }

    [Fact]
    public void Properties_CanBeModified()
    {
        // Arrange
        var request = new ChatContextCreateRequest();
        var newDate = new DateTime(2024, 3, 16, 15, 45, 0);

        // Act
        request.MessageId = 456;
        request.MessageText = "Updated message";
        request.MessageDate = newDate;

        // Assert
        Assert.Equal(456, request.MessageId);
        Assert.Equal("Updated message", request.MessageText);
        Assert.Equal(newDate, request.MessageDate);
    }

    [Fact]
    public void MessageText_CanBeSetToNull()
    {
        // Arrange
        var request = new ChatContextCreateRequest
        {
            MessageText = "Initial message"
        };

        // Act
        request.MessageText = null;

        // Assert
        Assert.Null(request.MessageText);
    }
} 