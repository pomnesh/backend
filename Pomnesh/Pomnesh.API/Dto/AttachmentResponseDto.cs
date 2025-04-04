using System.ComponentModel.DataAnnotations;

namespace Pomnesh.API.Dto;

public enum AttachmentTypeDto
{
    [Display(Name = "Photo")]
    Photo = 0,
    [Display(Name = "Video")]
    Video = 1,
    [Display(Name = "Audio")]
    Audio = 2,
    [Display(Name = "AudioMessage")]
    AudioMessage = 3,
    [Display(Name = "Document")]
    Document = 4,
}

public class AttachmentResponseDto
{
    public long Id { get; set; }

    public AttachmentTypeDto Type { get; set; }

    public long FileId { get; set; }

    public long OwnerId { get; set; }

    public string? OriginalLink { get; set; }

    public long ContextId { get; set; }
}