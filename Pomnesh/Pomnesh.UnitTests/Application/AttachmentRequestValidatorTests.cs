using FluentValidation.TestHelper;
using Pomnesh.API.Validators;
using Pomnesh.Application.Models;
using Pomnesh.Domain.Enum;

namespace Pomnesh.UnitTests.Application;

public class AttachmentRequestValidatorTests
{
    private readonly AttachmentCreateRequestValidator _createValidator;
    private readonly AttachmentUpdateRequestValidator _updateValidator;

    public AttachmentRequestValidatorTests()
    {
        _createValidator = new AttachmentCreateRequestValidator();
        _updateValidator = new AttachmentUpdateRequestValidator();
    }

    [Fact]
    public void AttachmentCreateRequest_ValidData_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new AttachmentCreateRequest
        {
            FileId = 1,
            OwnerId = 1,
            ContextId = 1,
            OriginalLink = "https://example.com",
            Type = AttachmentType.Photo
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void AttachmentCreateRequest_InvalidFileId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new AttachmentCreateRequest
        {
            FileId = 0,
            OwnerId = 1,
            ContextId = 1,
            Type = AttachmentType.Photo
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FileId)
            .WithErrorMessage("FileId must be a positive number.");
    }

    [Fact]
    public void AttachmentCreateRequest_InvalidOwnerId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new AttachmentCreateRequest
        {
            FileId = 1,
            OwnerId = 0,
            ContextId = 1,
            Type = AttachmentType.Photo
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OwnerId)
            .WithErrorMessage("OwnerId must be a positive number.");
    }

    [Fact]
    public void AttachmentCreateRequest_ZeroContextId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new AttachmentCreateRequest
        {
            FileId = 1,
            OwnerId = 1,
            ContextId = 0,
            Type = AttachmentType.Photo
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ContextId)
            .WithErrorMessage("ContextId must be a positive number.");
    }

    [Fact]
    public void AttachmentCreateRequest_InvalidOriginalLink_ShouldHaveValidationError()
    {
        // Arrange
        var request = new AttachmentCreateRequest
        {
            FileId = 1,
            OwnerId = 1,
            ContextId = 1,
            OriginalLink = "invalid-url",
            Type = AttachmentType.Photo
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OriginalLink)
            .WithErrorMessage("OriginalLink must be a valid absolute URL when provided.");
    }

    [Fact]
    public void AttachmentUpdateRequest_ValidData_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new AttachmentUpdateRequest
        {
            Id = 1,
            FileId = 1,
            OwnerId = 1,
            ContextId = 1,
            OriginalLink = "https://example.com",
            Type = AttachmentType.Photo
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void AttachmentUpdateRequest_InvalidId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new AttachmentUpdateRequest
        {
            Id = 0,
            FileId = 1,
            OwnerId = 1,
            ContextId = 1,
            Type = AttachmentType.Photo
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required for update.");
    }

    [Fact]
    public void AttachmentUpdateRequest_InvalidContextId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new AttachmentUpdateRequest
        {
            Id = 1,
            FileId = 1,
            OwnerId = 1,
            ContextId = 0,
            Type = AttachmentType.Photo
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ContextId)
            .WithErrorMessage("ContextId must be a positive number.");
    }

    [Fact]
    public void AttachmentUpdateRequest_InvalidType_ShouldHaveValidationError()
    {
        // Arrange
        var request = new AttachmentUpdateRequest
        {
            Id = 1,
            FileId = 1,
            OwnerId = 1,
            ContextId = 1,
            Type = (AttachmentType)999 // Invalid enum value
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Type)
            .WithErrorMessage("Type must be a valid attachment type.");
    }
} 