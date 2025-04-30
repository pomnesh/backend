using Pomnesh.Application.Exceptions;
using Xunit;

namespace Pomnesh.UnitTests.Application.Exceptions;

public class BaseApiExceptionTests
{
    [Fact]
    public void BaseApiException_IsException()
    {
        // Arrange
        var exception = new AttachmentNotFoundError(123);

        // Act & Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void BaseApiException_DefaultConstructor_InitializesCorrectly()
    {
        // Arrange
        var exception = new AttachmentNotFoundError(123);

        // Act & Assert
        Assert.NotNull(exception);
        Assert.True(exception is BaseApiException);
    }

    [Fact]
    public void BaseApiException_Properties_AreAbstract()
    {
        // Arrange
        var exception = new AttachmentNotFoundError(123);

        // Act & Assert
        Assert.True(typeof(BaseApiException).GetProperty(nameof(BaseApiException.StatusCode))?.GetGetMethod()?.IsAbstract ?? false);
        Assert.True(typeof(BaseApiException).GetProperty(nameof(BaseApiException.Description))?.GetGetMethod()?.IsAbstract ?? false);
    }

    [Fact]
    public void BaseApiException_Properties_AreVirtual()
    {
        // Arrange
        var exception = new AttachmentNotFoundError(123);

        // Act & Assert
        Assert.True(typeof(BaseApiException).GetProperty(nameof(BaseApiException.StatusCode))?.GetSetMethod()?.IsVirtual ?? false);
        Assert.True(typeof(BaseApiException).GetProperty(nameof(BaseApiException.Description))?.GetSetMethod()?.IsVirtual ?? false);
    }
} 