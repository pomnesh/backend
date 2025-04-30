using Microsoft.AspNetCore.Mvc;
using Moq;
using Pomnesh.API.Controllers;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;

namespace Pomnesh.UnitTests.Application;

public class AttachmentControllerTests
{
    private readonly Mock<IAttachmentService> _mockAttachmentService;
    private readonly AttachmentController _controller;

    public AttachmentControllerTests()
    {
        _mockAttachmentService = new Mock<IAttachmentService>();
        _controller = new AttachmentController(_mockAttachmentService.Object);
    }

    [Fact]
    public async Task CreateAttachment_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var request = new AttachmentCreateRequest();
        var expectedId = 1;
        _mockAttachmentService.Setup(x => x.Create(request))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _controller.CreateAttachment(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<BaseApiResponse<int>>(createdResult.Value);
        Assert.Equal(expectedId, response.Payload);
        Assert.Equal(nameof(AttachmentController.GetAttachment), createdResult.ActionName);
    }

    [Fact]
    public async Task GetAttachment_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var id = 1L;
        var expectedAttachment = new AttachmentResponse();
        _mockAttachmentService.Setup(x => x.Get(id))
            .ReturnsAsync(expectedAttachment);

        // Act
        var result = await _controller.GetAttachment(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse<AttachmentResponse>>(okResult.Value);
        Assert.Equal(expectedAttachment, response.Payload);
    }

    [Fact]
    public async Task GetAttachment_NonExistingId_ThrowsAttachmentNotFoundError()
    {
        // Arrange
        var id = 1L;
        _mockAttachmentService.Setup(x => x.Get(id))
            .ReturnsAsync((AttachmentResponse?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AttachmentNotFoundError>(() => _controller.GetAttachment(id));
    }

    [Fact]
    public async Task GetAll_ExistingAttachments_ReturnsOkResult()
    {
        // Arrange
        var expectedAttachments = new List<AttachmentResponse> { new() };
        _mockAttachmentService.Setup(x => x.GetAll())
            .ReturnsAsync(expectedAttachments);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<BaseApiResponse<IEnumerable<AttachmentResponse>>>(okResult.Value);
        Assert.Equal(expectedAttachments, response.Payload);
    }

    [Fact]
    public async Task UpdateAttachment_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var request = new AttachmentUpdateRequest();
        _mockAttachmentService.Setup(x => x.Update(request))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateAttachment(request);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAttachment_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var id = 1L;
        _mockAttachmentService.Setup(x => x.Delete(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteAttachment(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
} 