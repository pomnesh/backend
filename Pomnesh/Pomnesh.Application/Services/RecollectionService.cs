using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Interfaces;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class RecollectionService(IBaseRepository<Recollection> recollectionRepository) : IBaseService<RecollectionCreateDto, Recollection, RecollectionUpdateDto>
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

    public async Task Update(RecollectionUpdateDto data)
    {
        var recollection = new Recollection
        {
            Id = data.Id,
            UserId = data.UserId,
            DownloadLink = data.DownloadLink
        };
        await recollectionRepository.Update(recollection);
    }

    public async Task Delete(long id)
    {
        await recollectionRepository.Delete(id);
    }
}