namespace Pomnesh.Application.Interfaces;

public interface IBaseService<TCreateDto, TUpdateDto, TMappedEntity>
{
    Task<int> Create(TCreateDto data);
    Task<TMappedEntity?> Get(long id);
    Task Update(TUpdateDto data);
    Task<IEnumerable<TMappedEntity>> GetAll();
    Task Delete(long id);
}