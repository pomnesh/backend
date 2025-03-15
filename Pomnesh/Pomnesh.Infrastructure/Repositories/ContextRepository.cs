using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Infrastructure.Repositories;
//
// public class ContextRepository(ApplicationDbContext db) : IBaseRepository<Context>
// {
//     public async Task<long> Create(Context entity)
//     {
//         await db.Contexts.AddAsync(entity);
//         await db.SaveChangesAsync();
//         return entity.Id;
//     }
//     
//     public async Task Delete(Context entity)
//     {
//         db.Contexts.Remove(entity);
//         await db.SaveChangesAsync();
//     }
//     
//     public async Task<Context> Update(Context entity)
//     {
//         db.Contexts.Update(entity);
//         await db.SaveChangesAsync();
//
//         return entity;
//     }
//
//     public async Task<Context?> GetById(long id)
//     {
//         return await db.Contexts.FindAsync(id);
//     }
// }