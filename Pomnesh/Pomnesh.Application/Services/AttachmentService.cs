using Pomnesh.API.Dto;
using Pomnesh.API.Models;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;
using Pomnesh.Domain.Entity;
using Pomnesh.Domain.Enum;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class AttachmentService(IBaseRepository<Attachment> attachmentRepository, IBaseRepository<ChatContext> chatContextRepository) : IAttachmentService
{
    public async Task<int> Create(AttachmentCreateRequest data)
    {
        var context = await chatContextRepository.GetById(data.ContextId);
        if (context == null)
            throw new ContextNotFoundError(data.ContextId);

        var attachment = new Attachment
        {
            Type = (AttachmentType)data.Type,
            FileId = data.FileId,
            OwnerId = data.OwnerId,
            OriginalLink = data.OriginalLink,
            ContextId = data.ContextId
        };

        return await attachmentRepository.Add(attachment);
    }

    public async Task<AttachmentResponse?> Get(long id)
    {
        var result = await attachmentRepository.GetById(id);
        if (result == null)
            return null;

        return new AttachmentResponse
        {
            Id = result.Id,
            Type = (AttachmentTypeDto)result.Type,
            FileId = result.FileId,
            OwnerId = result.OwnerId,
            OriginalLink = result.OriginalLink,
            ContextId = result.ContextId
        };
    }

    public async Task<IEnumerable<AttachmentResponse>> GetAll()
    {
        var result = await attachmentRepository.GetAll();

        var attachmentResponse = new List<AttachmentResponse>();
        foreach (var attachment in result)
        {
            var responseDto = new AttachmentResponse
            {
                Id = attachment.Id,
                Type = (AttachmentTypeDto)attachment.Type,
                FileId = attachment.FileId,
                OwnerId = attachment.OwnerId,
                OriginalLink = attachment.OriginalLink,
                ContextId = attachment.ContextId
            
            };
            attachmentResponse.Add(responseDto);
        }
        
        return attachmentResponse;
    }

    public async Task Update(AttachmentUpdateRequest data)
    {  
        var attachment = await attachmentRepository.GetById(data.Id);
        if (attachment == null)
            throw new AttachmentNotFoundError(data.Id);
        
        var context = await chatContextRepository.GetById(data.ContextId);
        if (context == null)
            throw new ContextNotFoundError(data.ContextId);
            
        var updatedAttachment = new Attachment
        {
            Id = data.Id,
            Type = (AttachmentType)data.Type,
            FileId = data.FileId,
            OwnerId = data.OwnerId,
            OriginalLink = data.OriginalLink,
            ContextId = data.ContextId
        
        };

        await attachmentRepository.Update(updatedAttachment);
    }

    public async Task Delete(long id)
    {
        var attachment = await attachmentRepository.GetById(id);
        if (attachment == null)
            throw new AttachmentNotFoundError(id);

        await attachmentRepository.Delete(id);
    }
} 