using Pomnesh.API.Dto;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Models;

namespace Pomnesh.Application.Interfaces;

public interface IChatContextService : IBaseService<ChatContextCreateRequest, ChatContextUpdateRequest, ChatContextResponse>
{
}