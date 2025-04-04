using Pomnesh.Domain.Entity;

namespace Pomnesh.Infrastructure.Interfaces;

public interface IBaseRepository<T>
{
    Task<int> Add(T entity);
    Task<T?> GetById(long id);
    Task<IEnumerable<T>> GetAll();
}