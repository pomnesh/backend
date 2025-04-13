using Pomnesh.API.Dto;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class RecollectionService(IBaseRepository<Recollection> recollectionRepository, IBaseRepository<User> userRepository) : IRecollectionService
{
    public async Task<int> Create(RecollectionCreateDto data)
    {
        var user = await userRepository.GetById(data.UserId);
        if (user == null)
            throw new UserNotFoundError(data.UserId);
        
        var recollection = new Recollection
        {
            UserId = data.UserId,
            DownloadLink = data.DownloadLink
        };

        return await recollectionRepository.Add(recollection);
    }

    public async Task<RecollectionResponse?> Get(long id)
    {
        var result = await recollectionRepository.GetById(id);
        if (result == null)
            throw new RecollectionNotFoundError(id); 
        
        return new RecollectionResponse
        {
            Id = result.Id,
            UserId = result.UserId,
            CreatedAt = result.CreatedAt,
            DownloadLink = result.DownloadLink
        };
    }
    
    public async Task<IEnumerable<RecollectionResponse>> GetAll()
    {
        var result = await recollectionRepository.GetAll();
        
        var recollectionResponse = new List<RecollectionResponse>();
        foreach (var recollection in result)
        {
            var responseDto = new RecollectionResponse
            {
                Id = recollection.Id,
                UserId = recollection.UserId,
                CreatedAt = recollection.CreatedAt,
                DownloadLink = recollection.DownloadLink
            };
            recollectionResponse.Add(responseDto);
        }

        return recollectionResponse;
    }

    public async Task Update(RecollectionUpdateDto data)
    {
        var recollection = await recollectionRepository.GetById(data.Id);
        if (recollection == null)
            throw new RecollectionNotFoundError(data.Id); 
        
        var user = await userRepository.GetById(data.UserId);
        if (user == null)
            throw new UserNotFoundError(data.UserId);
        
        var updatedRecollection = new Recollection
        {
            Id = data.Id,
            UserId = data.UserId,
            DownloadLink = data.DownloadLink
        };

        await recollectionRepository.Update(updatedRecollection);
    }

    public async Task Delete(long id)
    {
        var recollection = await recollectionRepository.GetById(id);
        if (recollection == null)
            throw new RecollectionNotFoundError(id);
        
        await recollectionRepository.Delete(id);
    }
}