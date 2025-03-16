using Pomnesh.Application.Dto;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class UsersService(IBaseRepository<User> usersRepository)
{
    public async Task Create(UserCreateDto data)
    {
        var user = new User
        {
            VkId = data.VkId,
            VkToken = data.VkToken
        };

        await usersRepository.AddAsync(user);
    }

    public async Task<User?> Get(long id)
    {
        var result = await usersRepository.GetById(id);
        return result;
    }
}