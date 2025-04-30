using Moq;
using Pomnesh.API.Dto;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;
using Xunit;

namespace Pomnesh.UnitTests.Application.Services;

public class ChatContextServiceTests
{
    private readonly Mock<IBaseRepository<ChatContext>> _mockRepository;
    private readonly ChatContextService _chatContextService;

    public ChatContextServiceTests()
    {
        _mockRepository = new Mock<IBaseRepository<ChatContext>>();
        _chatContextService = new ChatContextService(_mockRepository.Object);
    }

    [Fact]
    public async Task Create_ValidRequest_ReturnsContextId()
    {
        // Arrange
        var request = new ChatContextCreateRequest
        {
            MessageId = 123,
            MessageText = "Test message",
            MessageDate = new DateTime(2024, 3, 15, 14, 30, 0)
        };
        var expectedContextId = 1;
        _mockRepository.Setup(r => r.Add(It.IsAny<ChatContext>()))
            .ReturnsAsync(expectedContextId);

        // Act
        var result = await _chatContextService.Create(request);

        // Assert
        Assert.Equal(expectedContextId, result);
        _mockRepository.Verify(r => r.Add(It.Is<ChatContext>(c => 
            c.MessageId == request.MessageId &&
            c.MessageText == request.MessageText &&
            c.MessageDate == request.MessageDate)), Times.Once);
    }

    [Fact]
    public async Task Get_ExistingContext_ReturnsChatContextResponse()
    {
        // Arrange
        var contextId = 1;
        var chatContext = new ChatContext
        {
            Id = contextId,
            MessageId = 123,
            MessageText = "Test message",
            MessageDate = new DateTime(2024, 3, 15, 14, 30, 0)
        };
        _mockRepository.Setup(r => r.GetById(contextId))
            .ReturnsAsync(chatContext);

        // Act
        var result = await _chatContextService.Get(contextId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(chatContext.Id, result.Id);
        Assert.Equal(chatContext.MessageId, result.MessageId);
        Assert.Equal(chatContext.MessageText, result.MessageText);
        Assert.Equal(chatContext.MessageDate, result.MessageDate);
    }

    [Fact]
    public async Task Get_NonExistingContext_ThrowsContextNotFoundError()
    {
        // Arrange
        var contextId = 1;
        _mockRepository.Setup(r => r.GetById(contextId))
            .ReturnsAsync((ChatContext?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ContextNotFoundError>(() => _chatContextService.Get(contextId));
    }

    [Fact]
    public async Task GetAll_ReturnsAllChatContexts()
    {
        // Arrange
        var chatContexts = new List<ChatContext>
        {
            new() 
            { 
                Id = 1, 
                MessageId = 123, 
                MessageText = "Message 1", 
                MessageDate = new DateTime(2024, 3, 15, 14, 30, 0) 
            },
            new() 
            { 
                Id = 2, 
                MessageId = 456, 
                MessageText = "Message 2", 
                MessageDate = new DateTime(2024, 3, 16, 15, 45, 0) 
            }
        };
        _mockRepository.Setup(r => r.GetAll())
            .ReturnsAsync(chatContexts);

        // Act
        var result = await _chatContextService.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        var resultList = result.ToList();
        Assert.Equal(chatContexts[0].Id, resultList[0].Id);
        Assert.Equal(chatContexts[0].MessageId, resultList[0].MessageId);
        Assert.Equal(chatContexts[0].MessageText, resultList[0].MessageText);
        Assert.Equal(chatContexts[0].MessageDate, resultList[0].MessageDate);
        Assert.Equal(chatContexts[1].Id, resultList[1].Id);
        Assert.Equal(chatContexts[1].MessageId, resultList[1].MessageId);
        Assert.Equal(chatContexts[1].MessageText, resultList[1].MessageText);
        Assert.Equal(chatContexts[1].MessageDate, resultList[1].MessageDate);
    }

    [Fact]
    public async Task Update_ExistingContext_UpdatesSuccessfully()
    {
        // Arrange
        var request = new ChatContextUpdateRequest
        {
            Id = 1,
            MessageId = 789,
            MessageText = "Updated message",
            MessageDate = new DateTime(2024, 3, 17, 16, 0, 0)
        };
        var existingContext = new ChatContext { Id = request.Id };
        
        _mockRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync(existingContext);
        _mockRepository.Setup(r => r.Update(It.IsAny<ChatContext>()))
            .Returns(Task.CompletedTask);

        // Act
        await _chatContextService.Update(request);

        // Assert
        _mockRepository.Verify(r => r.Update(It.Is<ChatContext>(c => 
            c.Id == request.Id &&
            c.MessageId == request.MessageId &&
            c.MessageText == request.MessageText &&
            c.MessageDate == request.MessageDate)), Times.Once);
    }

    [Fact]
    public async Task Update_NonExistingContext_ThrowsContextNotFoundError()
    {
        // Arrange
        var request = new ChatContextUpdateRequest { Id = 1 };
        _mockRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync((ChatContext?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ContextNotFoundError>(() => _chatContextService.Update(request));
    }

    [Fact]
    public async Task Delete_ExistingContext_DeletesSuccessfully()
    {
        // Arrange
        var contextId = 1;
        var existingContext = new ChatContext { Id = contextId };
        
        _mockRepository.Setup(r => r.GetById(contextId))
            .ReturnsAsync(existingContext);
        _mockRepository.Setup(r => r.Delete(contextId))
            .Returns(Task.CompletedTask);

        // Act
        await _chatContextService.Delete(contextId);

        // Assert
        _mockRepository.Verify(r => r.Delete(contextId), Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingContext_ThrowsContextNotFoundError()
    {
        // Arrange
        var contextId = 1;
        _mockRepository.Setup(r => r.GetById(contextId))
            .ReturnsAsync((ChatContext?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ContextNotFoundError>(() => _chatContextService.Delete(contextId));
    }
} 