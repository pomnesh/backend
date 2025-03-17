using Pomnesh.Domain.Entity;

namespace Pomnesh.Infrastructure.Interfaces;

public interface IBaseRepository<T>
{
    Task<int> AddAsync(T entity);
    Task<T?> GetById(long id);

    // Task DeleteById(long id);
    //
    // Task<T> Update(T entity);
}