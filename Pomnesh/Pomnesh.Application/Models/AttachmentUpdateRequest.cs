using Pomnesh.Domain.Enum;

namespace Pomnesh.Application.Models;

public class AttachmentUpdateRequest
{
    public long Id { get; set; }
    public AttachmentType Type { get; set; }
    public long FileId { get; set; }
    public long OwnerId { get; set; }
    public string? OriginalLink { get; set; }
    public long ContextId { get; set; }
}