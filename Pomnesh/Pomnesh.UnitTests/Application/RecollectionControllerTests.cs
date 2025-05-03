using Microsoft.AspNetCore.Mvc;
using Moq;
using Pomnesh.API.Controllers;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;

namespace Pomnesh.UnitTests.Application;

public class RecollectionControllerTests
{
    private readonly Mock<IRecollectionService> _mockRecollectionService;
    private readonly RecollectionController _controller;

    public RecollectionControllerTests()
    {
        _mockRecollectionService = new Mock<IRecollectionService>();
        _controller = new RecollectionController(_mockRecollectionService.Object);
    }

    [Fact]
    public async Task CreateRecollection_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var request = new RecollectionCreateRequest
        {
            UserId = 1,
            DownloadLink = "https://example.com/download"
        };
        var expectedId = 1;
        _mockRecollectionService.Setup(x => x.Create(request))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _controller.CreateRecollection(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<BaseApiResponse<int>>(createdResult.Value);
        Assert.Equal(expectedId, response.Payload);
        Assert.Equal(nameof(RecollectionController.GetRecollection), createdResult.ActionName);
    }

    [Fact]
    public async Task GetRecollection_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var id = 1L;
        var expectedRecollection = new RecollectionResponse
        {
            Id = id,
            UserId = 1,
            DownloadLink = "https://example.com/download"
        };
        _mockRecollectionService.Setup(x => x.Get(id))
            .ReturnsAsync(expectedRecollection);

        // Act
        var result = await _controller.GetRecollection(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse<RecollectionResponse>>(okResult.Value);
        Assert.Equal(expectedRecollection, response.Payload);
    }

    [Fact]
    public async Task GetRecollection_NonExistingId_ThrowsRecollectionNotFoundError()
    {
        // Arrange
        var id = 1L;
        _mockRecollectionService.Setup(x => x.Get(id))
            .ReturnsAsync((RecollectionResponse?)null);

        // Act & Assert
        await Assert.ThrowsAsync<RecollectionNotFoundError>(() => _controller.GetRecollection(id));
    }

    [Fact]
    public async Task GetAll_ExistingRecollections_ReturnsOkResult()
    {
        // Arrange
        var expectedRecollections = new List<RecollectionResponse>
        {
            new()
            {
                Id = 1,
                UserId = 1,
                DownloadLink = "https://example.com/download"
            }
        };
        _mockRecollectionService.Setup(x => x.GetAll())
            .ReturnsAsync(expectedRecollections);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse<IEnumerable<RecollectionResponse>>>(okResult.Value);
        Assert.Equal(expectedRecollections, response.Payload);
    }

    [Fact]
    public async Task UpdateRecollection_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            Id = 1,
            UserId = 1,
            DownloadLink = "https://example.com/download"
        };
        _mockRecollectionService.Setup(x => x.Update(request))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateRecollection(request);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteRecollection_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var id = 1L;
        _mockRecollectionService.Setup(x => x.Delete(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteRecollection(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
} 