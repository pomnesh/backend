using Pomnesh.API.Dto;
using Pomnesh.Application.Models;

namespace Pomnesh.Application.Interfaces;

public interface IUserService : IBaseService<UserCreateRequest, UserUpdateRequest, UserResponse>
{
    
}