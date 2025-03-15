namespace Pomnesh.Infrastructure.Interfaces;

public interface IBaseRepository<T>
{
    Task AddAsync(T entity);

    // Task DeleteById(long id);
    //
    // Task<T> Update(T entity);
}