using Pomnesh.Domain.Enum;

namespace Pomnesh.Application.Dto;

public class AttachmentCreateDto
{
    public AttachmentType Type;

    public long FileId;

    public long OwnerId;

    public string? OriginalLink;

    public long ContextId;
}