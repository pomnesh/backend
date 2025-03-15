using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Infrastructure.Repositories;

public class ResultRepository(ApplicationDbContext db) : IBaseRepository<Result>
{
    public async Task<long> Create(Result entity)
    {
        await db.Results.AddAsync(entity);
        await db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(Result entity)
    {
        db.Results.Remove(entity);
        await db.SaveChangesAsync();
    }
    
    public async Task<Result> Update(Result entity)
    {
        db.Results.Update(entity);
        await db.SaveChangesAsync();

        return entity;
    }
    
    public async Task<Result?> GetById(long id)
    {
        return await db.Results.FindAsync(id);
    }
}