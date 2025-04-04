using Pomnesh.Application.Dto;
using Pomnesh.Application.Interfaces;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class RecollectionService(IBaseRepository<Recollection> recollectionRepository) : IBaseService<RecollectionCreateDto, Recollection>
{
    public async Task<int> Create(RecollectionCreateDto data)
    {
        var recollection = new Recollection
        {
            UserId = data.UserId,
            DownloadLink = data.DownloadLink
        };

        return await recollectionRepository.Add(recollection);
    }

    public async Task<Recollection?> Get(long id)
    {
        var result = await recollectionRepository.GetById(id);
        return result;
    }
    
    public async Task<IEnumerable<Recollection>> GetAll()
    {
        var result = await recollectionRepository.GetAll();
        return result;
    }
}