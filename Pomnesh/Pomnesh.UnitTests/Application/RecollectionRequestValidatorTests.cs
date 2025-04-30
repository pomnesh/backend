using FluentValidation.TestHelper;
using Pomnesh.API.Validators;
using Pomnesh.Application.Models;
using Xunit;

namespace Pomnesh.UnitTests.Application;

public class RecollectionRequestValidatorTests
{
    private readonly RecollectionCreateRequestValidator _createValidator;
    private readonly RecollectionUpdateRequestValidator _updateValidator;

    public RecollectionRequestValidatorTests()
    {
        _createValidator = new RecollectionCreateRequestValidator();
        _updateValidator = new RecollectionUpdateRequestValidator();
    }

    [Fact]
    public void RecollectionCreateRequest_ValidData_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new RecollectionCreateRequest
        {
            UserId = 1,
            DownloadLink = "https://example.com/download"
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void RecollectionCreateRequest_ZeroUserId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RecollectionCreateRequest
        {
            UserId = 0,
            DownloadLink = "https://example.com/download"
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("User ID must be a positive number.");
    }

    [Fact]
    public void RecollectionCreateRequest_EmptyDownloadLink_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RecollectionCreateRequest
        {
            UserId = 1,
            DownloadLink = string.Empty
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DownloadLink)
            .WithErrorMessage("Download link is required.");
    }

    [Fact]
    public void RecollectionCreateRequest_InvalidDownloadLink_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RecollectionCreateRequest
        {
            UserId = 1,
            DownloadLink = "not-a-url"
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DownloadLink)
            .WithErrorMessage("Download link must be a valid URL.");
    }

    [Fact]
    public void RecollectionUpdateRequest_ValidData_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            Id = 1,
            UserId = 1,
            DownloadLink = "https://example.com/download"
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void RecollectionUpdateRequest_ZeroId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            Id = 0,
            UserId = 1,
            DownloadLink = "https://example.com/download"
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("ID must be a positive number.");
    }

    [Fact]
    public void RecollectionUpdateRequest_ZeroUserId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            Id = 1,
            UserId = 0,
            DownloadLink = "https://example.com/download"
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("User ID must be a positive number.");
    }

    [Fact]
    public void RecollectionUpdateRequest_EmptyDownloadLink_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            Id = 1,
            UserId = 1,
            DownloadLink = string.Empty
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DownloadLink)
            .WithErrorMessage("Download link is required.");
    }

    [Fact]
    public void RecollectionUpdateRequest_InvalidDownloadLink_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RecollectionUpdateRequest
        {
            Id = 1,
            UserId = 1,
            DownloadLink = "not-a-url"
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DownloadLink)
            .WithErrorMessage("Download link must be a valid URL.");
    }
} 