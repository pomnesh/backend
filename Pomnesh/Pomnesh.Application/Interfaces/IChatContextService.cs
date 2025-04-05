using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Domain.Entity;

namespace Pomnesh.Application.Interfaces;

public interface IChatContextService : IBaseService<ChatContextCreateDto, ChatContext, ChatContextUpdateDto>
{
}