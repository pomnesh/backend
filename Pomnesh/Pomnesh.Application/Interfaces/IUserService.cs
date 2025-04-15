using Pomnesh.API.Dto;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;

namespace Pomnesh.Application.Interfaces;

public interface IUserService : IBaseService<UserCreateDto, UserUpdateDto, UserResponse>
{
    
}