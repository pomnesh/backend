using Pomnesh.Application.Dto;
using Pomnesh.Application.Interfaces;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class AttachmentService(IBaseRepository<Attachment> attachmentRepository) : IBaseService<AttachmentCreateDto, Attachment>
{
    public async Task<int> Create(AttachmentCreateDto data)
    {
        var attachment = new Attachment
        {
            Type = data.Type,
            FileId = data.FileId,
            OwnerId = data.OwnerId,
            OriginalLink = data.OriginalLink,
            ContextId = data.ContextId
        };

        return await attachmentRepository.Add(attachment);
    }

    public async Task<Attachment?> Get(long id)
    {
        var result = await attachmentRepository.GetById(id);
        return result;
    }
}