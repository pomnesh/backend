using Microsoft.AspNetCore.Mvc;
using Moq;
using Pomnesh.API.Controllers;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;

namespace Pomnesh.UnitTests.Application;

public class ChatContextControllerTests
{
    private readonly Mock<IChatContextService> _mockChatContextService;
    private readonly ChatContextController _controller;

    public ChatContextControllerTests()
    {
        _mockChatContextService = new Mock<IChatContextService>();
        _controller = new ChatContextController(_mockChatContextService.Object);
    }

    [Fact]
    public async Task CreateContext_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var request = new ChatContextCreateRequest();
        var expectedId = 1;
        _mockChatContextService.Setup(x => x.Create(request))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _controller.CreateContext(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<BaseApiResponse<int>>(createdResult.Value);
        Assert.Equal(expectedId, response.Payload);
        Assert.Equal(nameof(ChatContextController.GetContext), createdResult.ActionName);
    }

    [Fact]
    public async Task GetContext_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var id = 1L;
        var expectedContext = new ChatContextResponse();
        _mockChatContextService.Setup(x => x.Get(id))
            .ReturnsAsync(expectedContext);

        // Act
        var result = await _controller.GetContext(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse<ChatContextResponse>>(okResult.Value);
        Assert.Equal(expectedContext, response.Payload);
    }

    [Fact]
    public async Task GetContext_NonExistingId_ThrowsContextNotFoundError()
    {
        // Arrange
        var id = 1L;
        _mockChatContextService.Setup(x => x.Get(id))
            .ReturnsAsync((ChatContextResponse?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ContextNotFoundError>(() => _controller.GetContext(id));
    }

    [Fact]
    public async Task GetAll_ExistingContexts_ReturnsOkResult()
    {
        // Arrange
        var expectedContexts = new List<ChatContextResponse> { new() };
        _mockChatContextService.Setup(x => x.GetAll())
            .ReturnsAsync(expectedContexts);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse<IEnumerable<ChatContextResponse>>>(okResult.Value);
        Assert.Equal(expectedContexts, response.Payload);
    }

    [Fact]
    public async Task UpdateChatContext_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var request = new ChatContextUpdateRequest();
        _mockChatContextService.Setup(x => x.Update(request))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateChatContext(request);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteChatContext_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var id = 1L;
        _mockChatContextService.Setup(x => x.Delete(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteChatContext(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
} 