using Pomnesh.Application.Models;
using Xunit;

namespace Pomnesh.UnitTests.Application.Models;

public class UserCreateRequestTests
{
    [Fact]
    public void Constructor_Default_InitializesPropertiesWithDefaultValues()
    {
        // Arrange & Act
        var request = new UserCreateRequest();

        // Assert
        Assert.Equal(0, request.VkId);
        Assert.Null(request.VkToken);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var request = new UserCreateRequest
        {
            VkId = 123456789,
            VkToken = "test_vk_token"
        };

        // Act & Assert
        Assert.Equal(123456789, request.VkId);
        Assert.Equal("test_vk_token", request.VkToken);
    }

    [Fact]
    public void Properties_CanBeModified()
    {
        // Arrange
        var request = new UserCreateRequest();

        // Act
        request.VkId = 987654321;
        request.VkToken = "new_vk_token";

        // Assert
        Assert.Equal(987654321, request.VkId);
        Assert.Equal("new_vk_token", request.VkToken);
    }

    [Fact]
    public void VkToken_CanBeSetToNull()
    {
        // Arrange
        var request = new UserCreateRequest
        {
            VkToken = "initial_token"
        };

        // Act
        request.VkToken = null;

        // Assert
        Assert.Null(request.VkToken);
    }
} 