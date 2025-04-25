using Pomnesh.API.Dto;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Models;
using Pomnesh.Domain.Entity;

namespace Pomnesh.Application.Interfaces;

public interface IRecollectionService : IBaseService<RecollectionCreateRequest, RecollectionUpdateRequest, RecollectionResponse>
{
    
}