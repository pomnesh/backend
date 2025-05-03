using Pomnesh.API.Dto;
using Xunit;

namespace Pomnesh.UnitTests.Application.Responses;

public class UserResponseTests
{
    [Fact]
    public void Constructor_Default_InitializesPropertiesWithDefaultValues()
    {
        // Arrange & Act
        var response = new UserResponse();

        // Assert
        Assert.Equal(0, response.Id);
        Assert.Equal(0, response.VkId);
        Assert.Null(response.VkToken);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var response = new UserResponse
        {
            Id = 1,
            VkId = 123456789,
            VkToken = "test_token_123"
        };

        // Act & Assert
        Assert.Equal(1, response.Id);
        Assert.Equal(123456789, response.VkId);
        Assert.Equal("test_token_123", response.VkToken);
    }

    [Fact]
    public void Properties_CanBeModified()
    {
        // Arrange
        var response = new UserResponse();

        // Act
        response.Id = 2;
        response.VkId = 987654321;
        response.VkToken = "new_token_456";

        // Assert
        Assert.Equal(2, response.Id);
        Assert.Equal(987654321, response.VkId);
        Assert.Equal("new_token_456", response.VkToken);
    }

    [Fact]
    public void Id_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new UserResponse();

        // Act
        response.Id = 100;
        response.Id = 200;

        // Assert
        Assert.Equal(200, response.Id);
    }

    [Fact]
    public void VkId_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new UserResponse();

        // Act
        response.VkId = 1000000000;
        response.VkId = 2000000000;

        // Assert
        Assert.Equal(2000000000, response.VkId);
    }

    [Fact]
    public void VkToken_CanBeSetToNull()
    {
        // Arrange
        var response = new UserResponse
        {
            VkToken = "initial_token"
        };

        // Act
        response.VkToken = null;

        // Assert
        Assert.Null(response.VkToken);
    }

    [Fact]
    public void VkToken_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new UserResponse();

        // Act
        response.VkToken = "first_token";
        response.VkToken = "second_token";

        // Assert
        Assert.Equal("second_token", response.VkToken);
    }

    [Fact]
    public void VkToken_CanBeSetToEmptyString()
    {
        // Arrange
        var response = new UserResponse();

        // Act
        response.VkToken = string.Empty;

        // Assert
        Assert.Equal(string.Empty, response.VkToken);
    }
} 