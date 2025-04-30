using Pomnesh.API.Dto;
using Xunit;

namespace Pomnesh.UnitTests.Application.Responses;

public class RecollectionResponseTests
{
    [Fact]
    public void Constructor_Default_InitializesPropertiesWithDefaultValues()
    {
        // Arrange & Act
        var response = new RecollectionResponse
        {
            DownloadLink = "https://example.com/download"
        };

        // Assert
        Assert.Equal(0, response.Id);
        Assert.Equal(0, response.UserId);
        Assert.Equal(default(DateTime), response.CreatedAt);
        Assert.Equal("https://example.com/download", response.DownloadLink);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var testDate = new DateTime(2024, 3, 15, 14, 30, 0);
        var response = new RecollectionResponse
        {
            Id = 1,
            UserId = 1,
            CreatedAt = testDate,
            DownloadLink = "https://example.com/download"
        };

        // Act & Assert
        Assert.Equal(1, response.Id);
        Assert.Equal(1, response.UserId);
        Assert.Equal(testDate, response.CreatedAt);
        Assert.Equal("https://example.com/download", response.DownloadLink);
    }

    [Fact]
    public void Properties_CanBeModified()
    {
        // Arrange
        var response = new RecollectionResponse
        {
            DownloadLink = "https://example.com/download"
        };
        var newDate = new DateTime(2024, 3, 16, 15, 45, 0);

        // Act
        response.Id = 2;
        response.UserId = 2;
        response.CreatedAt = newDate;
        response.DownloadLink = "https://example.com/new-download";

        // Assert
        Assert.Equal(2, response.Id);
        Assert.Equal(2, response.UserId);
        Assert.Equal(newDate, response.CreatedAt);
        Assert.Equal("https://example.com/new-download", response.DownloadLink);
    }

    [Fact]
    public void CreatedAt_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new RecollectionResponse
        {
            DownloadLink = "https://example.com/download"
        };
        var date1 = new DateTime(2024, 1, 1);
        var date2 = new DateTime(2024, 12, 31);

        // Act
        response.CreatedAt = date1;
        response.CreatedAt = date2;

        // Assert
        Assert.Equal(date2, response.CreatedAt);
    }

    [Fact]
    public void Id_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new RecollectionResponse
        {
            DownloadLink = "https://example.com/download"
        };

        // Act
        response.Id = 100;
        response.Id = 200;

        // Assert
        Assert.Equal(200, response.Id);
    }

    [Fact]
    public void UserId_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new RecollectionResponse
        {
            DownloadLink = "https://example.com/download"
        };

        // Act
        response.UserId = 1000;
        response.UserId = 2000;

        // Assert
        Assert.Equal(2000, response.UserId);
    }

    [Fact]
    public void DownloadLink_CanBeSetToDifferentValues()
    {
        // Arrange
        var response = new RecollectionResponse
        {
            DownloadLink = "https://example.com/download"
        };

        // Act
        response.DownloadLink = "https://example.com/first";
        response.DownloadLink = "https://example.com/second";

        // Assert
        Assert.Equal("https://example.com/second", response.DownloadLink);
    }
} 