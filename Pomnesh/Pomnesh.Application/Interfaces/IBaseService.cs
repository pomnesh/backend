namespace Pomnesh.Application.Interfaces;

public interface IBaseService<TCreateDto, TEntity, TUpdateDto>
{
    Task<int> Create(TCreateDto data);
    Task<TEntity?> Get(long id);
    Task Update(TUpdateDto data);
}