using System.Runtime.CompilerServices;
using Pomnesh.Application.Models;
using Xunit;

namespace Pomnesh.UnitTests.Application.Models;

public class RecollectionUpdateRequestTests
{
    [Fact]
    public void Constructor_Default_InitializesPropertiesWithDefaultValues()
    {
        // Arrange & Act
        var request = new RecollectionUpdateRequest
        {
            DownloadLink = "https://example.com/download"
        };

        // Assert
        Assert.Equal(0, request.Id);
        Assert.Equal(0, request.UserId);
        Assert.Equal("https://example.com/download", request.DownloadLink);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            Id = 1,
            UserId = 1,
            DownloadLink = "https://example.com/download"
        };

        // Act & Assert
        Assert.Equal(1, request.Id);
        Assert.Equal(1, request.UserId);
        Assert.Equal("https://example.com/download", request.DownloadLink);
    }

    [Fact]
    public void Properties_CanBeModified()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            DownloadLink = "https://example.com/download"
        };

        // Act
        request.Id = 2;
        request.UserId = 2;
        request.DownloadLink = "https://example.com/new-download";

        // Assert
        Assert.Equal(2, request.Id);
        Assert.Equal(2, request.UserId);
        Assert.Equal("https://example.com/new-download", request.DownloadLink);
    }

    [Fact]
    public void DownloadLink_IsRequired()
    {
        // Arrange & Act
        var request = new RecollectionUpdateRequest
        {
            DownloadLink = "https://example.com/download"
        };

        // Assert
        Assert.NotNull(request.DownloadLink);
        Assert.NotEmpty(request.DownloadLink);
    }

    [Fact]
    public void Id_CanBeSetToDifferentValues()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            DownloadLink = "https://example.com/download"
        };

        // Act
        request.Id = 100;
        request.Id = 200;

        // Assert
        Assert.Equal(200, request.Id);
    }
} 