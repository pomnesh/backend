namespace Pomnesh.Infrastructure.Interfaces;

public interface IBaseRepository<T>
{
    Task Create(T entity);

    Task Delete(T entity);

    Task<T> Update(T entity);
}