using Pomnesh.API.Dto;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Interfaces;
using Pomnesh.Domain.Entity;
using Pomnesh.Domain.Enum;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class AttachmentService(IBaseRepository<Attachment> attachmentRepository) : IBaseService<AttachmentCreateDto, Attachment, AttachmentUpdateDto>
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

    public async Task<IEnumerable<Attachment>> GetAll()
    {
        var result = await attachmentRepository.GetAll();
        return result;
    }

    public async Task Update(AttachmentUpdateDto data)
    {
        var attachment = new Attachment
        {
            Id = data.Id,
            Type = (AttachmentType)data.Type,
            FileId = data.FileId,
            OwnerId = data.OwnerId,
            OriginalLink = data.OriginalLink,
            ContextId = data.ContextId

        };

        await attachmentRepository.Update(attachment);
    }

    public async Task Delete(long id)
    {
        await attachmentRepository.Delete(id);
    }
} 