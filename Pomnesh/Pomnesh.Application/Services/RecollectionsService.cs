using Pomnesh.Application.Dto;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class RecollectionsService(IBaseRepository<Recollection> recollectionRepository)
{
    public async Task Create(RecollectionCreateDto data)
    {
        var recollection = new Recollection
        {
            UserId = data.UserId,
            DownloadLink = data.DownloadLink
        };

        await recollectionRepository.AddAsync(recollection);
    }

    public async Task<Recollection?> Get(long id)
    {
        var result = await recollectionRepository.GetById(id);
        return result;
    }
}