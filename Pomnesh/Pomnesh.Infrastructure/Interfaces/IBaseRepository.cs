namespace Pomnesh.Infrastructure.Interfaces;

public interface IBaseRepository<T>
{
    Task<long> Create(T entity);

    Task Delete(T entity);

    Task<T> Update(T entity);
}