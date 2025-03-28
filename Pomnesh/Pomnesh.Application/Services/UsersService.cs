﻿using Pomnesh.Application.Dto;
using Pomnesh.Application.Interfaces;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class UsersService(IBaseRepository<User> usersRepository) : IBaseService<UserCreateDto, User>
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
}