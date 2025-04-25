using Pomnesh.API.Dto;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class RecollectionService(IBaseRepository<Recollection> recollectionRepository, IBaseRepository<User> userRepository) : IRecollectionService
{
    public async Task<int> Create(RecollectionCreateRequest request)
    {
        var user = await userRepository.GetById(request.UserId);
        if (user == null)
            throw new UserNotFoundError(request.UserId);
        
        var recollection = new Recollection
        {
            UserId = request.UserId,
            DownloadLink = request.DownloadLink
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

    public async Task Update(RecollectionUpdateRequest request)
    {
        var recollection = await recollectionRepository.GetById(request.Id);
        if (recollection == null)
            throw new RecollectionNotFoundError(request.Id); 
        
        var user = await userRepository.GetById(request.UserId);
        if (user == null)
            throw new UserNotFoundError(request.UserId);
        
        var updatedRecollection = new Recollection
        {
            Id = request.Id,
            UserId = request.UserId,
            DownloadLink = request.DownloadLink
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