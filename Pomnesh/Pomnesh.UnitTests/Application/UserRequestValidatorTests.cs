using FluentValidation.TestHelper;
using Pomnesh.API.Validators;
using Pomnesh.Application.Models;

namespace Pomnesh.UnitTests.Application;

public class UserRequestValidatorTests
{
    private readonly UserCreateRequestValidator _createValidator;
    private readonly UserUpdateRequestValidator _updateValidator;

    public UserRequestValidatorTests()
    {
        _createValidator = new UserCreateRequestValidator();
        _updateValidator = new UserUpdateRequestValidator();
    }

    [Fact]
    public void UserCreateRequest_ValidData_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new UserCreateRequest
        {
            VkId = 1
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UserCreateRequest_InvalidVkId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new UserCreateRequest
        {
            VkId = 0
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VkId)
            .WithErrorMessage("VkId must be a positive number.");
    }

    [Fact]
    public void UserUpdateRequest_ValidData_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new UserUpdateRequest
        {
            Id = 1,
            VkId = 1
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UserUpdateRequest_InvalidId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new UserUpdateRequest
        {
            Id = 0,
            VkId = 1
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required for update.");
    }

    [Fact]
    public void UserUpdateRequest_InvalidVkId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new UserUpdateRequest
        {
            Id = 1,
            VkId = 0
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VkId)
            .WithErrorMessage("VkId must be a positive number.");
    }
} 