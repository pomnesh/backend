namespace Pomnesh.Application.Interfaces;

public interface IBaseService<TCreateDto, TEntity>
{
    Task<int> Create(TCreateDto data);
    Task<TEntity?> Get(long id);
}