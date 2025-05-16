using Pomnesh.Domain.Entity;

namespace Pomnesh.Infrastructure.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetByUsername(string username);
} 