using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Infrastructure.Repositories;

public class AttachmentRepository(ApplicationDbContext db) : IBaseRepository<Attachment>
{
    public async Task<long> Create(Attachment entity)
    {
        await db.Attachments.AddAsync(entity);
        await db.SaveChangesAsync();
        return entity.Id;
    }
    
    public async Task Delete(Attachment entity)
    {
        db.Attachments.Remove(entity);
        await db.SaveChangesAsync();
    }
    
    public async Task<Attachment> Update(Attachment entity)
    {
        db.Attachments.Update(entity);
        await db.SaveChangesAsync();

        return entity;
    }

    public async Task<Attachment?> GetById(long id)
    {
        return await db.Attachments.FindAsync(id);
    }
}