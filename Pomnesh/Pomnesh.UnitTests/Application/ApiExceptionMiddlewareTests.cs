using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Json;
using Moq;
using Pomnesh.API.Middlewares;
using Pomnesh.Application.Exceptions;
using System.Text.Json;
using System.Text;

namespace Pomnesh.UnitTests.Application;

public class ApiExceptionMiddlewareTests
{
    private readonly Mock<RequestDelegate> _mockNext;
    private readonly ApiExceptionMiddleware _middleware;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<HttpResponse> _mockResponse;
    private readonly MemoryStream _responseBody;
    private readonly IServiceProvider _serviceProvider;
    private int _statusCode;

    public ApiExceptionMiddlewareTests()
    {
        _mockNext = new Mock<RequestDelegate>();
        _middleware = new ApiExceptionMiddleware(_mockNext.Object);
        _mockHttpContext = new Mock<HttpContext>();
        _mockResponse = new Mock<HttpResponse>();
        _responseBody = new MemoryStream();
        _statusCode = 0;

        // Set up services for JSON serialization
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
        _serviceProvider = services.BuildServiceProvider();

        _mockHttpContext.Setup(x => x.Response)
            .Returns(_mockResponse.Object);
        _mockResponse.Setup(x => x.Body)
            .Returns(_responseBody);
        _mockHttpContext.Setup(x => x.RequestServices)
            .Returns(_serviceProvider);
        _mockResponse.Setup(x => x.HttpContext)
            .Returns(_mockHttpContext.Object);
        _mockResponse.SetupSet(x => x.StatusCode = It.IsAny<int>())
            .Callback<int>(code => _statusCode = code);
        _mockResponse.SetupProperty(x => x.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_NoException_ExecutesNextDelegate()
    {
        // Arrange
        _mockNext.Setup(x => x(It.IsAny<HttpContext>()))
            .Returns(Task.CompletedTask);

        // Act
        await _middleware.InvokeAsync(_mockHttpContext.Object);

        // Assert
        _mockNext.Verify(x => x(_mockHttpContext.Object), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_NonApiException_ThrowsOriginalException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockNext.Setup(x => x(It.IsAny<HttpContext>()))
            .ThrowsAsync(exception);

        // Act & Assert
        var thrownException = await Assert.ThrowsAsync<Exception>(
            () => _middleware.InvokeAsync(_mockHttpContext.Object));
        Assert.Same(exception, thrownException);
    }

    private class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Error { get; set; } = string.Empty;
    }
} 