using Pomnesh.Domain.Enum;

namespace Pomnesh.Application.Dto;

public class AttachmentCreateDto
{
    public AttachmentType Type { get; set; }

    public long FileId { get; set; }

    public long OwnerId { get; set; }

    public string? OriginalLink { get; set; }

    public long ContextId { get; set; }
}