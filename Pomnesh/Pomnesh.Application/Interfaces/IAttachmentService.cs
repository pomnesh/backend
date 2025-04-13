using Pomnesh.API.Dto;
using Pomnesh.Application.Dto;
using Pomnesh.Domain.Entity;

namespace Pomnesh.Application.Interfaces;

public interface IAttachmentService : IBaseService<AttachmentCreateDto, AttachmentUpdateDto, AttachmentResponse>
{
}