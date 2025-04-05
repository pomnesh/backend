using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Interfaces;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class UserService(IBaseRepository<User> usersRepository) : IUserService
{
    public async Task<int> Create(UserCreateDto data)
    {
        var user = new User
        {
            VkId = data.VkId,
            VkToken = data.VkToken
        };

        return await usersRepository.Add(user);
    }

    public async Task<User?> Get(long id)
    {
        var result = await usersRepository.GetById(id);
        return result;
    }
    
    public async Task<IEnumerable<User>> GetAll()
    {
        var result = await usersRepository.GetAll();
        return result;
    }

    public async Task Update(UserUpdateDto data)
    {
        var user = new User
        {
            Id = data.Id,
            VkId = data.VkId,
            VkToken = data.VkToken
        };
        await usersRepository.Update(user);
    }

    public async Task Delete(long id)
    {
        await usersRepository.Delete(id);
    }
}