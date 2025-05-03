using Pomnesh.Application.Models;
using Xunit;

namespace Pomnesh.UnitTests.Application.Models;

public class UserUpdateRequestTests
{
    [Fact]
    public void Constructor_Default_InitializesPropertiesWithDefaultValues()
    {
        // Arrange & Act
        var request = new UserUpdateRequest();

        // Assert
        Assert.Equal(0, request.Id);
        Assert.Equal(0, request.VkId);
        Assert.Null(request.VkToken);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var request = new UserUpdateRequest
        {
            Id = 1,
            VkId = 123456789,
            VkToken = "test_vk_token"
        };

        // Act & Assert
        Assert.Equal(1, request.Id);
        Assert.Equal(123456789, request.VkId);
        Assert.Equal("test_vk_token", request.VkToken);
    }

    [Fact]
    public void Properties_CanBeModified()
    {
        // Arrange
        var request = new UserUpdateRequest();

        // Act
        request.Id = 2;
        request.VkId = 987654321;
        request.VkToken = "new_vk_token";

        // Assert
        Assert.Equal(2, request.Id);
        Assert.Equal(987654321, request.VkId);
        Assert.Equal("new_vk_token", request.VkToken);
    }

    [Fact]
    public void VkToken_CanBeSetToNull()
    {
        // Arrange
        var request = new UserUpdateRequest
        {
            VkToken = "initial_token"
        };

        // Act
        request.VkToken = null;

        // Assert
        Assert.Null(request.VkToken);
    }

    [Fact]
    public void Id_CanBeSetToDifferentValues()
    {
        // Arrange
        var request = new UserUpdateRequest();

        // Act
        request.Id = 100;
        request.Id = 200;

        // Assert
        Assert.Equal(200, request.Id);
    }
} 