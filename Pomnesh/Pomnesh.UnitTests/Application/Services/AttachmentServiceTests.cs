using Moq;
using Pomnesh.API.Dto;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;
using Pomnesh.Domain.Enum;
using Pomnesh.Infrastructure.Interfaces;
using Xunit;

namespace Pomnesh.UnitTests.Application.Services;

public class AttachmentServiceTests
{
    private readonly Mock<IBaseRepository<Attachment>> _mockAttachmentRepository;
    private readonly Mock<IBaseRepository<ChatContext>> _mockChatContextRepository;
    private readonly AttachmentService _attachmentService;

    public AttachmentServiceTests()
    {
        _mockAttachmentRepository = new Mock<IBaseRepository<Attachment>>();
        _mockChatContextRepository = new Mock<IBaseRepository<ChatContext>>();
        _attachmentService = new AttachmentService(_mockAttachmentRepository.Object, _mockChatContextRepository.Object);
    }

    [Fact]
    public async Task Create_ValidRequest_ReturnsAttachmentId()
    {
        // Arrange
        var request = new AttachmentCreateRequest
        {
            Type = AttachmentType.Photo,
            FileId = 123,
            OwnerId = 1,
            OriginalLink = "https://example.com/photo.jpg",
            ContextId = 1
        };
        var expectedAttachmentId = 1;
        var context = new ChatContext { Id = request.ContextId };
        
        _mockChatContextRepository.Setup(r => r.GetById(request.ContextId))
            .ReturnsAsync(context);
        _mockAttachmentRepository.Setup(r => r.Add(It.IsAny<Attachment>()))
            .ReturnsAsync(expectedAttachmentId);

        // Act
        var result = await _attachmentService.Create(request);

        // Assert
        Assert.Equal(expectedAttachmentId, result);
        _mockAttachmentRepository.Verify(r => r.Add(It.Is<Attachment>(a => 
            a.Type == AttachmentType.Photo &&
            a.FileId == 123 &&
            a.OwnerId == request.OwnerId &&
            a.OriginalLink == request.OriginalLink &&
            a.ContextId == request.ContextId)), Times.Once);
    }

    [Fact]
    public async Task Create_NonExistingContext_ThrowsContextNotFoundError()
    {
        // Arrange
        var request = new AttachmentCreateRequest { ContextId = 1 };
        _mockChatContextRepository.Setup(r => r.GetById(request.ContextId))
            .ReturnsAsync((ChatContext?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ContextNotFoundError>(() => _attachmentService.Create(request));
    }

    [Fact]
    public async Task Get_ExistingAttachment_ReturnsAttachmentResponse()
    {
        // Arrange
        var attachmentId = 1;
        var attachment = new Attachment
        {
            Id = attachmentId,
            Type = AttachmentType.Photo,
            FileId = 123,
            OwnerId = 1,
            OriginalLink = "https://example.com/photo.jpg",
            ContextId = 1
        };
        _mockAttachmentRepository.Setup(r => r.GetById(attachmentId))
            .ReturnsAsync(attachment);

        // Act
        var result = await _attachmentService.Get(attachmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(attachment.Id, result.Id);
        Assert.Equal((AttachmentTypeDto)attachment.Type, result.Type);
        Assert.Equal(attachment.FileId, result.FileId);
        Assert.Equal(attachment.OwnerId, result.OwnerId);
        Assert.Equal(attachment.OriginalLink, result.OriginalLink);
        Assert.Equal(attachment.ContextId, result.ContextId);
    }

    [Fact]
    public async Task Get_NonExistingAttachment_ReturnsNull()
    {
        // Arrange
        var attachmentId = 1;
        _mockAttachmentRepository.Setup(r => r.GetById(attachmentId))
            .ReturnsAsync((Attachment?)null);

        // Act
        var result = await _attachmentService.Get(attachmentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ReturnsAllAttachments()
    {
        // Arrange
        var attachments = new List<Attachment>
        {
            new() 
            { 
                Id = 1, 
                Type = AttachmentType.Photo, 
                FileId = 123, 
                OwnerId = 1, 
                OriginalLink = "link1", 
                ContextId = 1 
            },
            new() 
            { 
                Id = 2, 
                Type = AttachmentType.Document, 
                FileId = 456, 
                OwnerId = 2, 
                OriginalLink = "link2", 
                ContextId = 2 
            }
        };
        _mockAttachmentRepository.Setup(r => r.GetAll())
            .ReturnsAsync(attachments);

        // Act
        var result = await _attachmentService.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        var resultList = result.ToList();
        Assert.Equal(attachments[0].Id, resultList[0].Id);
        Assert.Equal((AttachmentTypeDto)attachments[0].Type, resultList[0].Type);
        Assert.Equal(attachments[0].FileId, resultList[0].FileId);
        Assert.Equal(attachments[0].OwnerId, resultList[0].OwnerId);
        Assert.Equal(attachments[0].OriginalLink, resultList[0].OriginalLink);
        Assert.Equal(attachments[0].ContextId, resultList[0].ContextId);
        Assert.Equal(attachments[1].Id, resultList[1].Id);
        Assert.Equal((AttachmentTypeDto)attachments[1].Type, resultList[1].Type);
        Assert.Equal(attachments[1].FileId, resultList[1].FileId);
        Assert.Equal(attachments[1].OwnerId, resultList[1].OwnerId);
        Assert.Equal(attachments[1].OriginalLink, resultList[1].OriginalLink);
        Assert.Equal(attachments[1].ContextId, resultList[1].ContextId);
    }

    [Fact]
    public async Task Update_ValidRequest_UpdatesAttachment()
    {
        // Arrange
        var request = new AttachmentUpdateRequest
        {
            Id = 1,
            Type = AttachmentType.Photo,
            FileId = 456,
            OwnerId = 2,
            OriginalLink = "new_link",
            ContextId = 2
        };
        var existingAttachment = new Attachment { Id = request.Id };
        var context = new ChatContext { Id = request.ContextId };
        
        _mockAttachmentRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync(existingAttachment);
        _mockChatContextRepository.Setup(r => r.GetById(request.ContextId))
            .ReturnsAsync(context);
        _mockAttachmentRepository.Setup(r => r.Update(It.IsAny<Attachment>()))
            .Returns(Task.CompletedTask);

        // Act
        await _attachmentService.Update(request);

        // Assert
        _mockAttachmentRepository.Verify(r => r.Update(It.Is<Attachment>(a => 
            a.Id == request.Id &&
            a.Type == AttachmentType.Photo &&
            a.FileId == request.FileId &&
            a.OwnerId == request.OwnerId &&
            a.OriginalLink == request.OriginalLink &&
            a.ContextId == request.ContextId)), Times.Once);
    }

    [Fact]
    public async Task Update_NonExistingAttachment_ThrowsAttachmentNotFoundError()
    {
        // Arrange
        var request = new AttachmentUpdateRequest { Id = 1, ContextId = 1 };
        _mockAttachmentRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync((Attachment?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AttachmentNotFoundError>(() => _attachmentService.Update(request));
    }

    [Fact]
    public async Task Update_NonExistingContext_ThrowsContextNotFoundError()
    {
        // Arrange
        var request = new AttachmentUpdateRequest { Id = 1, ContextId = 1 };
        var existingAttachment = new Attachment { Id = request.Id };
        
        _mockAttachmentRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync(existingAttachment);
        _mockChatContextRepository.Setup(r => r.GetById(request.ContextId))
            .ReturnsAsync((ChatContext?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ContextNotFoundError>(() => _attachmentService.Update(request));
    }

    [Fact]
    public async Task Delete_ExistingAttachment_DeletesSuccessfully()
    {
        // Arrange
        var attachmentId = 1;
        var existingAttachment = new Attachment { Id = attachmentId };
        
        _mockAttachmentRepository.Setup(r => r.GetById(attachmentId))
            .ReturnsAsync(existingAttachment);
        _mockAttachmentRepository.Setup(r => r.Delete(attachmentId))
            .Returns(Task.CompletedTask);

        // Act
        await _attachmentService.Delete(attachmentId);

        // Assert
        _mockAttachmentRepository.Verify(r => r.Delete(attachmentId), Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingAttachment_ThrowsAttachmentNotFoundError()
    {
        // Arrange
        var attachmentId = 1;
        _mockAttachmentRepository.Setup(r => r.GetById(attachmentId))
            .ReturnsAsync((Attachment?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AttachmentNotFoundError>(() => _attachmentService.Delete(attachmentId));
    }
} 