using Pomnesh.API.Responses;
using Xunit;

namespace Pomnesh.UnitTests.Application.Responses;

public class BaseApiResponseTests
{
    [Fact]
    public void Constructor_Default_InitializesPayloadWithDefaultValue()
    {
        // Arrange & Act
        var response = new BaseApiResponse<string>();

        // Assert
        Assert.Null(response.Payload);
    }

    [Fact]
    public void Payload_CanBeSetAndRetrieved_WithString()
    {
        // Arrange
        var response = new BaseApiResponse<string>
        {
            Payload = "test payload"
        };

        // Act & Assert
        Assert.Equal("test payload", response.Payload);
    }

    [Fact]
    public void Payload_CanBeSetAndRetrieved_WithInt()
    {
        // Arrange
        var response = new BaseApiResponse<int>
        {
            Payload = 42
        };

        // Act & Assert
        Assert.Equal(42, response.Payload);
    }

    [Fact]
    public void Payload_CanBeSetAndRetrieved_WithObject()
    {
        // Arrange
        var testObject = new { Name = "Test", Value = 123 };
        var response = new BaseApiResponse<object>
        {
            Payload = testObject
        };

        // Act & Assert
        Assert.Equal(testObject, response.Payload);
    }

    [Fact]
    public void Payload_CanBeModified()
    {
        // Arrange
        var response = new BaseApiResponse<string>();

        // Act
        response.Payload = "initial value";
        response.Payload = "updated value";

        // Assert
        Assert.Equal("updated value", response.Payload);
    }

    [Fact]
    public void Payload_CanBeSetToNull()
    {
        // Arrange
        var response = new BaseApiResponse<string>
        {
            Payload = "initial value"
        };

        // Act
        response.Payload = null;

        // Assert
        Assert.Null(response.Payload);
    }

    [Fact]
    public void Payload_CanBeSetAndRetrieved_WithCustomClass()
    {
        // Arrange
        var testClass = new TestClass { Id = 1, Name = "Test" };
        var response = new BaseApiResponse<TestClass>
        {
            Payload = testClass
        };

        // Act & Assert
        Assert.Equal(testClass, response.Payload);
        Assert.Equal(1, response.Payload?.Id);
        Assert.Equal("Test", response.Payload?.Name);
    }

    private class TestClass
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
} 