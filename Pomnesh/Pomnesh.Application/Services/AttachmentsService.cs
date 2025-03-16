using Pomnesh.Application.Dto;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class AttachmentsService(IBaseRepository<Attachment> attachmentRepository)
{
    public async Task Create(AttachmentCreateDto data)
    {
        var attachment = new Attachment
        {
            Type = data.Type,
            FileId = data.FileId,
            OwnerId = data.OwnerId,
            OriginalLink = data.OriginalLink,
            ContextId = data.ContextId
        };

        await attachmentRepository.AddAsync(attachment);
    }

    public async Task<Attachment?> Get(long id)
    {
        var result = await attachmentRepository.GetById(id);
        return result;
    }
}