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

public class UserServiceTests
{
    private readonly Mock<IBaseRepository<User>> _mockRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IBaseRepository<User>>();
        _userService = new UserService(_mockRepository.Object);
    }

    [Fact]
    public async Task Create_ValidRequest_ReturnsUserId()
    {
        // Arrange
        var request = new UserCreateRequest
        {
            VkId = 123456789,
            VkToken = "test_token"
        };
        var expectedUserId = 1;
        _mockRepository.Setup(r => r.Add(It.IsAny<User>()))
            .ReturnsAsync(expectedUserId);

        // Act
        var result = await _userService.Create(request);

        // Assert
        Assert.Equal(expectedUserId, result);
        _mockRepository.Verify(r => r.Add(It.Is<User>(u => 
            u.VkId == request.VkId && 
            u.VkToken == request.VkToken)), Times.Once);
    }

    [Fact]
    public async Task Get_ExistingUser_ReturnsUserResponse()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            VkId = 123456789,
            VkToken = "test_token"
        };
        _mockRepository.Setup(r => r.GetById(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.Get(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.VkId, result.VkId);
        Assert.Equal(user.VkToken, result.VkToken);
    }

    [Fact]
    public async Task Get_NonExistingUser_ThrowsUserNotFoundError()
    {
        // Arrange
        var userId = 1;
        _mockRepository.Setup(r => r.GetById(userId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundError>(() => _userService.Get(userId));
    }

    [Fact]
    public async Task GetAll_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = 1, VkId = 123456789, VkToken = "token1" },
            new() { Id = 2, VkId = 987654321, VkToken = "token2" }
        };
        _mockRepository.Setup(r => r.GetAll())
            .ReturnsAsync(users);

        // Act
        var result = await _userService.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        var resultList = result.ToList();
        Assert.Equal(users[0].Id, resultList[0].Id);
        Assert.Equal(users[0].VkId, resultList[0].VkId);
        Assert.Equal(users[0].VkToken, resultList[0].VkToken);
        Assert.Equal(users[1].Id, resultList[1].Id);
        Assert.Equal(users[1].VkId, resultList[1].VkId);
        Assert.Equal(users[1].VkToken, resultList[1].VkToken);
    }

    [Fact]
    public async Task Update_ExistingUser_UpdatesSuccessfully()
    {
        // Arrange
        var request = new UserUpdateRequest
        {
            Id = 1,
            VkId = 123456789,
            VkToken = "new_token"
        };
        var existingUser = new User { Id = request.Id };
        _mockRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync(existingUser);
        _mockRepository.Setup(r => r.Update(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        await _userService.Update(request);

        // Assert
        _mockRepository.Verify(r => r.Update(It.Is<User>(u => 
            u.Id == request.Id && 
            u.VkId == request.VkId && 
            u.VkToken == request.VkToken)), Times.Once);
    }

    [Fact]
    public async Task Update_NonExistingUser_ThrowsUserNotFoundError()
    {
        // Arrange
        var request = new UserUpdateRequest { Id = 1 };
        _mockRepository.Setup(r => r.GetById(request.Id))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundError>(() => _userService.Update(request));
    }

    [Fact]
    public async Task Delete_ExistingUser_DeletesSuccessfully()
    {
        // Arrange
        var userId = 1;
        var existingUser = new User { Id = userId };
        _mockRepository.Setup(r => r.GetById(userId))
            .ReturnsAsync(existingUser);
        _mockRepository.Setup(r => r.Delete(userId))
            .Returns(Task.CompletedTask);

        // Act
        await _userService.Delete(userId);

        // Assert
        _mockRepository.Verify(r => r.Delete(userId), Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingUser_ThrowsUserNotFoundError()
    {
        // Arrange
        var userId = 1;
        _mockRepository.Setup(r => r.GetById(userId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundError>(() => _userService.Delete(userId));
    }
} 