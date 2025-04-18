﻿using Pomnesh.API.Dto;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Domain.Entity;

namespace Pomnesh.Application.Interfaces;

public interface IRecollectionService : IBaseService<RecollectionCreateDto, RecollectionUpdateDto, RecollectionResponse>
{
    
}