using Pomnesh.API.Dto;
using Pomnesh.API.Models;
using Pomnesh.Application.Models;

namespace Pomnesh.Application.Interfaces;

public interface IAttachmentService : IBaseService<AttachmentCreateRequest, AttachmentUpdateRequest, AttachmentResponse>
{
}