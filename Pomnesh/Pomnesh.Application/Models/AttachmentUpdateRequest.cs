namespace Pomnesh.API.Models;

public class AttachmentUpdateRequest
{
    public long Id { get; set; }
    public AttachmentCopyType Type { get; set; }
    public long? FileId { get; set; }
    public long? OwnerId { get; set; }
    public string? OriginalLink { get; set; }
    public long? ContextId { get; set; }
}