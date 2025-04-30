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

public class RecollectionServiceTests
{
    private readonly Mock<IBaseRepository<Recollection>> _mockRecollectionRepository;
    private readonly Mock<IBaseRepository<User>> _mockUserRepository;
    private readonly RecollectionService _recollectionService;

    public RecollectionServiceTests()
    {
        _mockRecollectionRepository = new Mock<IBaseRepository<Recollection>>();
        _mockUserRepository = new Mock<IBaseRepository<User>>();
        _recollectionService = new RecollectionService(_mockRecollectionRepository.Object, _mockUserRepository.Object);
    }

    [Fact]
    public async Task Create_ValidRequest_ReturnsRecollectionId()
    {
        // Arrange
        var request = new RecollectionCreateRequest
        {
            UserId = 1,
            DownloadLink = "https://example.com/download"
        };
        var expectedRecollectionId = 1;
        var user = new User { Id = request.UserId };
        
        _mockUserRepository.Setup(r => r.GetById(request.UserId))
            .ReturnsAsync(user);
        _mockRecollectionRepository.Setup(r => r.Add(It.IsAny<Recollection>()))
            .ReturnsAsync(expectedRecollectionId);

        // Act
        var result = await _recollectionService.Create(request);

        // Assert
        Assert.Equal(expectedRecollectionId, result);
        _mockRecollectionRepository.Verify(r => r.Add(It.Is<Recollection>(rec => 
            rec.UserId == request.UserId &&
            rec.DownloadLink == request.DownloadLink)), Times.Once);
    }

    [Fact]
    public async Task Create_NonExistingUser_ThrowsUserNotFoundError()
    {
        // Arrange
        var request = new RecollectionCreateRequest 
        { 
            UserId = 1,
            DownloadLink = "https://example.com/download"
        };
        _mockUserRepository.Setup(r => r.GetById(request.UserId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundError>(() => _recollectionService.Create(request));
    }

    [Fact]
    public async Task Get_ExistingRecollection_ReturnsRecollectionResponse()
    {
        // Arrange
        var recollectionId = 1;
        var recollection = new Recollection
        {
            Id = recollectionId,
            UserId = 1,
            CreatedAt = new DateTime(2024, 3, 15, 14, 30, 0),
            DownloadLink = "https://example.com/download"
        };
        _mockRecollectionRepository.Setup(r => r.GetById(recollectionId))
            .ReturnsAsync(recollection);

        // Act
        var result = await _recollectionService.Get(recollectionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(recollection.Id, result.Id);
        Assert.Equal(recollection.UserId, result.UserId);
        Assert.Equal(recollection.CreatedAt, result.CreatedAt);
        Assert.Equal(recollection.DownloadLink, result.DownloadLink);
    }

    [Fact]
    public async Task Get_NonExistingRecollection_ThrowsRecollectionNotFoundError()
    {
        // Arrange
        var recollectionId = 1;
        _mockRecollectionRepository.Setup(r => r.GetById(recollectionId))
            .ReturnsAsync((Recollection?)null);

        // Act & Assert
        await Assert.ThrowsAsync<RecollectionNotFoundError>(() => _recollectionService.Get(recollectionId));
    }

    [Fact]
    public async Task GetAll_ReturnsAllRecollections()
    {
        // Arrange
        var recollections = new List<Recollection>
        {
            new() 
            { 
                Id = 1, 
                UserId = 1, 
                CreatedAt = new DateTime(2024, 3, 15, 14, 30, 0), 
                DownloadLink = "link1" 
            },
            new() 
            { 
                Id = 2, 
                UserId = 2, 
                CreatedAt = new DateTime(2024, 3, 16, 15, 45, 0), 
                DownloadLink = "link2" 
            }
        };
        _mockRecollectionRepository.Setup(r => r.GetAll())
            .ReturnsAsync(recollections);

        // Act
        var result = await _recollectionService.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        var resultList = result.ToList();
        Assert.Equal(recollections[0].Id, resultList[0].Id);
        Assert.Equal(recollections[0].UserId, resultList[0].UserId);
        Assert.Equal(recollections[0].CreatedAt, resultList[0].CreatedAt);
        Assert.Equal(recollections[0].DownloadLink, resultList[0].DownloadLink);
        Assert.Equal(recollections[1].Id, resultList[1].Id);
        Assert.Equal(recollections[1].UserId, resultList[1].UserId);
        Assert.Equal(recollections[1].CreatedAt, resultList[1].CreatedAt);
        Assert.Equal(recollections[1].DownloadLink, resultList[1].DownloadLink);
    }

    [Fact]
    public async Task Update_ExistingRecollection_UpdatesSuccessfully()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            Id = 1,
            UserId = 2,
            DownloadLink = "https://example.com/new-download"
        };
        var existingRecollection = new Recollection { Id = request.Id, DownloadLink = "https://example.com/old-download" };
        var user = new User { Id = request.UserId };
        
        _mockRecollectionRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync(existingRecollection);
        _mockUserRepository.Setup(r => r.GetById(request.UserId))
            .ReturnsAsync(user);
        _mockRecollectionRepository.Setup(r => r.Update(It.IsAny<Recollection>()))
            .Returns(Task.CompletedTask);

        // Act
        await _recollectionService.Update(request);

        // Assert
        _mockRecollectionRepository.Verify(r => r.Update(It.Is<Recollection>(rec => 
            rec.Id == request.Id &&
            rec.UserId == request.UserId &&
            rec.DownloadLink == request.DownloadLink)), Times.Once);
    }

    [Fact]
    public async Task Update_NonExistingRecollection_ThrowsRecollectionNotFoundError()
    {
        // Arrange
        var request = new RecollectionUpdateRequest { Id = 1, UserId = 1, DownloadLink = "https://example.com/download" };
        _mockRecollectionRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync((Recollection?)null);

        // Act & Assert
        await Assert.ThrowsAsync<RecollectionNotFoundError>(() => _recollectionService.Update(request));
    }

    [Fact]
    public async Task Update_NonExistingUser_ThrowsUserNotFoundError()
    {
        // Arrange
        var request = new RecollectionUpdateRequest { Id = 1, UserId = 1, DownloadLink = "https://example.com/download" };
        var existingRecollection = new Recollection { Id = request.Id, DownloadLink = "https://example.com/old-download" };
        
        _mockRecollectionRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync(existingRecollection);
        _mockUserRepository.Setup(r => r.GetById(request.UserId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundError>(() => _recollectionService.Update(request));
    }

    [Fact]
    public async Task Delete_ExistingRecollection_DeletesSuccessfully()
    {
        // Arrange
        var recollectionId = 1;
        var existingRecollection = new Recollection { Id = recollectionId, DownloadLink = "https://example.com/download" };
        
        _mockRecollectionRepository.Setup(r => r.GetById(recollectionId))
            .ReturnsAsync(existingRecollection);
        _mockRecollectionRepository.Setup(r => r.Delete(recollectionId))
            .Returns(Task.CompletedTask);

        // Act
        await _recollectionService.Delete(recollectionId);

        // Assert
        _mockRecollectionRepository.Verify(r => r.Delete(recollectionId), Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingRecollection_ThrowsRecollectionNotFoundError()
    {
        // Arrange
        var recollectionId = 1;
        _mockRecollectionRepository.Setup(r => r.GetById(recollectionId))
            .ReturnsAsync((Recollection?)null);

        // Act & Assert
        await Assert.ThrowsAsync<RecollectionNotFoundError>(() => _recollectionService.Delete(recollectionId));
    }
} 