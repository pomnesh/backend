using Pomnesh.API.Dto;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Pomnesh.UnitTests.Application.Responses;

public class AttachmentResponseTests
{
    [Fact]
    public void Constructor_Default_InitializesPropertiesWithDefaultValues()
    {
        // Arrange & Act
        var response = new AttachmentResponse();

        // Assert
        Assert.Equal(0, response.Id);
        Assert.Equal(default(AttachmentTypeDto), response.Type);
        Assert.Equal(0, response.FileId);
        Assert.Equal(0, response.OwnerId);
        Assert.Null(response.OriginalLink);
        Assert.Equal(0, response.ContextId);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var response = new AttachmentResponse
        {
            Id = 1,
            Type = AttachmentTypeDto.Photo,
            FileId = 123,
            OwnerId = 456,
            OriginalLink = "https://example.com/image.jpg",
            ContextId = 789
        };

        // Act & Assert
        Assert.Equal(1, response.Id);
        Assert.Equal(AttachmentTypeDto.Photo, response.Type);
        Assert.Equal(123, response.FileId);
        Assert.Equal(456, response.OwnerId);
        Assert.Equal("https://example.com/image.jpg", response.OriginalLink);
        Assert.Equal(789, response.ContextId);
    }

    [Fact]
    public void Properties_CanBeModified()
    {
        // Arrange
        var response = new AttachmentResponse();

        // Act
        response.Id = 2;
        response.Type = AttachmentTypeDto.Video;
        response.FileId = 111;
        response.OwnerId = 222;
        response.OriginalLink = "https://example.com/video.mp4";
        response.ContextId = 333;

        // Assert
        Assert.Equal(2, response.Id);
        Assert.Equal(AttachmentTypeDto.Video, response.Type);
        Assert.Equal(111, response.FileId);
        Assert.Equal(222, response.OwnerId);
        Assert.Equal("https://example.com/video.mp4", response.OriginalLink);
        Assert.Equal(333, response.ContextId);
    }

    [Fact]
    public void OriginalLink_CanBeSetToNull()
    {
        // Arrange
        var response = new AttachmentResponse
        {
            OriginalLink = "https://example.com"
        };

        // Act
        response.OriginalLink = null;

        // Assert
        Assert.Null(response.OriginalLink);
    }

    [Fact]
    public void AttachmentTypeDto_Values_AreCorrect()
    {
        // Arrange & Act
        var photo = AttachmentTypeDto.Photo;
        var video = AttachmentTypeDto.Video;
        var audio = AttachmentTypeDto.Audio;
        var audioMessage = AttachmentTypeDto.AudioMessage;
        var document = AttachmentTypeDto.Document;

        // Assert
        Assert.Equal(0, (int)photo);
        Assert.Equal(1, (int)video);
        Assert.Equal(2, (int)audio);
        Assert.Equal(3, (int)audioMessage);
        Assert.Equal(4, (int)document);
    }

    [Fact]
    public void AttachmentTypeDto_DisplayNames_AreCorrect()
    {
        // Arrange
        var photoType = typeof(AttachmentTypeDto).GetField("Photo");
        var videoType = typeof(AttachmentTypeDto).GetField("Video");
        var audioType = typeof(AttachmentTypeDto).GetField("Audio");
        var audioMessageType = typeof(AttachmentTypeDto).GetField("AudioMessage");
        var documentType = typeof(AttachmentTypeDto).GetField("Document");

        // Act
        var photoDisplay = photoType?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault()?.Name;
        var videoDisplay = videoType?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault()?.Name;
        var audioDisplay = audioType?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault()?.Name;
        var audioMessageDisplay = audioMessageType?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault()?.Name;
        var documentDisplay = documentType?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault()?.Name;

        // Assert
        Assert.Equal("Photo", photoDisplay);
        Assert.Equal("Video", videoDisplay);
        Assert.Equal("Audio", audioDisplay);
        Assert.Equal("AudioMessage", audioMessageDisplay);
        Assert.Equal("Document", documentDisplay);
    }
} 