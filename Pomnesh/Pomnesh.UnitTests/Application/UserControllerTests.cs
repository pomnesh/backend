using Microsoft.AspNetCore.Mvc;
using Moq;
using Pomnesh.API.Controllers;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;

namespace Pomnesh.UnitTests.Application;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Fact]
    public async Task CreateUser_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var request = new UserCreateRequest();
        var expectedId = 1;
        _mockUserService.Setup(x => x.Create(request))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _controller.CreateUser(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<BaseApiResponse<int>>(createdResult.Value);
        Assert.Equal(expectedId, response.Payload);
        Assert.Equal(nameof(UserController.GetUserInfo), createdResult.ActionName);
    }

    [Fact]
    public async Task GetUserInfo_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var id = 1L;
        var expectedUser = new UserResponse();
        _mockUserService.Setup(x => x.Get(id))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUserInfo(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse<UserResponse>>(okResult.Value);
        Assert.Equal(expectedUser, response.Payload);
    }

    [Fact]
    public async Task GetUserInfo_NonExistingId_ThrowsUserNotFoundError()
    {
        // Arrange
        var id = 1L;
        _mockUserService.Setup(x => x.Get(id))
            .ReturnsAsync((UserResponse?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundError>(() => _controller.GetUserInfo(id));
    }

    [Fact]
    public async Task GetAll_ExistingUsers_ReturnsOkResult()
    {
        // Arrange
        var expectedUsers = new List<UserResponse> { new() };
        _mockUserService.Setup(x => x.GetAll())
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse<IEnumerable<UserResponse>>>(okResult.Value);
        Assert.Equal(expectedUsers, response.Payload);
    }

    [Fact]
    public async Task UpdateUser_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var request = new UserUpdateRequest();
        _mockUserService.Setup(x => x.Update(request))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateUser(request);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteUser_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var id = 1L;
        _mockUserService.Setup(x => x.Delete(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUser(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
} 