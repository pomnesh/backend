using Pomnesh.API.Dto;
using Pomnesh.API.Models;
using Pomnesh.Application.Dto;
using Pomnesh.Domain.Entity;

namespace Pomnesh.Application.Interfaces;

public interface IAttachmentService : IBaseService<AttachmentCreateRequest, AttachmentUpdateDto, AttachmentResponse>
{
}