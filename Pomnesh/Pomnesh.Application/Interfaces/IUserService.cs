using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Domain.Entity;

namespace Pomnesh.Application.Interfaces;

public interface IUserService : IBaseService<UserCreateDto, User, UserUpdateDto>
{
    
}