using Pomnesh.API.Dto;
using Xunit;

namespace Pomnesh.UnitTests.Application.Responses;

public class ChatContextResponseTests
{
    [Fact]
    public void Constructor_Default_InitializesPropertiesWithDefaultValues()
    {
        // Arrange & Act
        var response = new ChatContextResponse();

        // Assert
        Assert.Equal(0, response.Id);
        Assert.Equal(0, response.MessageId);
        Assert.Null(response.MessageText);
        Assert.Equal(default(DateTime), response.MessageDate);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var testDate = new DateTime(2024, 3, 15, 14, 30, 0);
        var response = new ChatContextResponse
        {
            Id = 1,
            MessageId = 123,
            MessageText = "Test message",
            MessageDate = testDate
        };

        // Act & Assert
        Assert.Equal(1, response.Id);
        Assert.Equal(123, response.MessageId);
        Assert.Equal("Test message", response.MessageText);
        Assert.Equal(testDate, response.MessageDate);
    }

    [Fact]
    public void Properties_CanBeModified()
    {
        // Arrange
        var response = new ChatContextResponse();
        var newDate = new DateTime(2024, 3, 16, 15, 45, 0);

        // Act
        response.Id = 2;
        response.MessageId = 456;
        response.MessageText = "Updated message";
        response.MessageDate = newDate;

        // Assert
        Assert.Equal(2, response.Id);
        Assert.Equal(456, response.MessageId);
        Assert.Equal("Updated message", response.MessageText);
        Assert.Equal(newDate, response.MessageDate);
    }

    [Fact]
    public void MessageText_CanBeSetToNull()
    {
        // Arrange
        var response = new ChatContextResponse
        {
            MessageText = "Initial message"
        };

        // Act
        response.MessageText = null;

        // Assert
        Assert.Null(response.MessageText);
    }

    [Fact]
    public void MessageDate_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new ChatContextResponse();
        var date1 = new DateTime(2024, 1, 1);
        var date2 = new DateTime(2024, 12, 31);

        // Act
        response.MessageDate = date1;
        response.MessageDate = date2;

        // Assert
        Assert.Equal(date2, response.MessageDate);
    }

    [Fact]
    public void Id_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new ChatContextResponse();

        // Act
        response.Id = 100;
        response.Id = 200;

        // Assert
        Assert.Equal(200, response.Id);
    }

    [Fact]
    public void MessageId_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new ChatContextResponse();

        // Act
        response.MessageId = 1000;
        response.MessageId = 2000;

        // Assert
        Assert.Equal(2000, response.MessageId);
    }
} 