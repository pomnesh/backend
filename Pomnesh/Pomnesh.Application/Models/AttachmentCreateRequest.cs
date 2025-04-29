namespace Pomnesh.Application.Models;
using Pomnesh.Domain.Enum;

public class AttachmentCreateRequest
{
    public AttachmentType Type { get; set; }
    public long FileId { get; set; }
    public long OwnerId { get; set; }
    public string? OriginalLink { get; set; }
    public long ContextId { get; set; }
}