using Pomnesh.Application.Exceptions;
using Xunit;

namespace Pomnesh.UnitTests.Application.Exceptions;

public class UserNotFoundErrorTests
{
    [Fact]
    public void Constructor_WithUserId_SetsPropertiesCorrectly()
    {
        // Arrange
        const long userId = 101;

        // Act
        var exception = new UserNotFoundError(userId);

        // Assert
        Assert.Equal(userId, exception.UserId);
        Assert.Equal(404, exception.StatusCode);
        Assert.Equal($"The user with ID '101' was not found", exception.Description);
    }

    [Fact]
    public void StatusCode_Setter_ThrowsNotImplementedException()
    {
        // Arrange
        var exception = new UserNotFoundError(101);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => exception.StatusCode = 500);
    }

    [Fact]
    public void Description_Setter_ThrowsNotImplementedException()
    {
        // Arrange
        var exception = new UserNotFoundError(101);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => exception.Description = "New description");
    }

    [Fact]
    public void UserId_IsReadOnly()
    {
        // Arrange
        const long userId = 101;
        var exception = new UserNotFoundError(userId);

        // Act & Assert
        Assert.Equal(userId, exception.UserId);
        // Verify that UserId is read-only by checking if it has a setter
        var property = typeof(UserNotFoundError).GetProperty(nameof(UserNotFoundError.UserId));
        Assert.NotNull(property);
        Assert.Null(property.GetSetMethod());
    }
} 