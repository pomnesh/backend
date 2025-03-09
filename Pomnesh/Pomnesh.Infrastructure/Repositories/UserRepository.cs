using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext db) : IBaseRepository<User>
{
    public async Task Create(User entity)
    {
        await db.Users.AddAsync(entity);
        await db.SaveChangesAsync();
    }

    public async Task Delete(User entity)
    {
        db.Users.Remove(entity);
        await db.SaveChangesAsync();
    }
    
    public async Task<User> Update(User entity)
    {
        db.Users.Update(entity);
        await db.SaveChangesAsync();

        return entity;
    }
    
    public async Task<User?> GetById(long id)
    {
        return await db.Users.FindAsync(id);
    }
}