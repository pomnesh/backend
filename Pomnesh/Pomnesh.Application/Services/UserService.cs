using Pomnesh.API.Dto;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class UserService(IBaseRepository<User> usersRepository) : IUserService
{
    public async Task<int> Create(UserCreateRequest request)
    {
        var newUser = new User
        {
            VkId = request.VkId,
            VkToken = request.VkToken
        };

        return await usersRepository.Add(newUser);
    }

    public async Task<UserResponse?> Get(long id)
    {
        var result = await usersRepository.GetById(id);
        if (result == null)
            throw new UserNotFoundError(id);

        return new UserResponse
        {
            Id = result.Id,
            VkId = result.VkId,
            VkToken = result.VkToken,
        };
    }

    public async Task<IEnumerable<UserResponse>> GetAll()
    {
        var result = await usersRepository.GetAll();

        var userResponse = new List<UserResponse>();
        foreach (var user in result)
        {
            var responseDto = new UserResponse
            {
                Id = user.Id,
                VkId = user.VkId,
                VkToken = user.VkToken,
            };
            userResponse.Add(responseDto);
        }
        
        return userResponse;
    }

    public async Task Update(UserUpdateRequest request)
    {
        var user = await usersRepository.GetById(request.Id);
        if (user == null)
            throw new UserNotFoundError(request.Id);
        
        var updatedUser = new User
        {
            Id = request.Id,
            VkId = request.VkId,
            VkToken = request.VkToken
        };

        await usersRepository.Update(updatedUser);
    }

    public async Task Delete(long id)
    {
        var user = await usersRepository.GetById(id);
        if (user == null)
            throw new UserNotFoundError(id);

        await usersRepository.Delete(id);
    }
}