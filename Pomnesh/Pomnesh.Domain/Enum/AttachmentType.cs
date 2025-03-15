using System.ComponentModel.DataAnnotations;

namespace Pomnesh.Domain.Enum;

public enum AttachmentType
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
    
    // implement new types in the future
}