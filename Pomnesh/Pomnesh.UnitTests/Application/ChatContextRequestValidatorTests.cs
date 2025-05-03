using FluentValidation.TestHelper;
using Pomnesh.API.Validators;
using Pomnesh.Application.Models;

namespace Pomnesh.UnitTests.Application;

public class ChatContextRequestValidatorTests
{
    private readonly ChatContextCreateRequestValidator _createValidator;
    private readonly ChatContextUpdateRequestValidator _updateValidator;

    public ChatContextRequestValidatorTests()
    {
        _createValidator = new ChatContextCreateRequestValidator();
        _updateValidator = new ChatContextUpdateRequestValidator();
    }

    [Fact]
    public void ChatContextCreateRequest_ValidData_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new ChatContextCreateRequest
        {
            MessageId = 1,
            MessageText = "Test message",
            MessageDate = DateTime.UtcNow
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ChatContextCreateRequest_InvalidMessageId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new ChatContextCreateRequest
        {
            MessageId = 0,
            MessageText = "Test message",
            MessageDate = DateTime.UtcNow
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MessageId)
            .WithErrorMessage("MessageId must be a positive number.");
    }

    [Fact]
    public void ChatContextCreateRequest_NullMessageText_ShouldHaveValidationError()
    {
        // Arrange
        var request = new ChatContextCreateRequest
        {
            MessageId = 1,
            MessageText = null,
            MessageDate = DateTime.UtcNow
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MessageText)
            .WithErrorMessage("MessageText must be provided.");
    }

    [Fact]
    public void ChatContextCreateRequest_DefaultMessageDate_ShouldHaveValidationError()
    {
        // Arrange
        var request = new ChatContextCreateRequest
        {
            MessageId = 1,
            MessageText = "Test message",
            MessageDate = default
        };

        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MessageDate)
            .WithErrorMessage("MessageDate must be provided.");
    }

    [Fact]
    public void ChatContextUpdateRequest_ValidData_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new ChatContextUpdateRequest
        {
            Id = 1,
            MessageId = 1,
            MessageText = "Test message",
            MessageDate = DateTime.UtcNow
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ChatContextUpdateRequest_InvalidId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new ChatContextUpdateRequest
        {
            Id = 0,
            MessageId = 1,
            MessageText = "Test message",
            MessageDate = DateTime.UtcNow
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required for update.");
    }

    [Fact]
    public void ChatContextUpdateRequest_InvalidMessageId_ShouldHaveValidationError()
    {
        // Arrange
        var request = new ChatContextUpdateRequest
        {
            Id = 1,
            MessageId = 0,
            MessageText = "Test message",
            MessageDate = DateTime.UtcNow
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MessageId)
            .WithErrorMessage("MessageId must be a positive number.");
    }

    [Fact]
    public void ChatContextUpdateRequest_NullMessageText_ShouldHaveValidationError()
    {
        // Arrange
        var request = new ChatContextUpdateRequest
        {
            Id = 1,
            MessageId = 1,
            MessageText = null,
            MessageDate = DateTime.UtcNow
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MessageText)
            .WithErrorMessage("MessageText must be provided.");
    }

    [Fact]
    public void ChatContextUpdateRequest_DefaultMessageDate_ShouldHaveValidationError()
    {
        // Arrange
        var request = new ChatContextUpdateRequest
        {
            Id = 1,
            MessageId = 1,
            MessageText = "Test message",
            MessageDate = default
        };

        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MessageDate)
            .WithErrorMessage("MessageDate must be provided.");
    }
} 