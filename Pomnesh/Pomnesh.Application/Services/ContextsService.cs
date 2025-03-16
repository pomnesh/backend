using Pomnesh.Application.Dto;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class ContextsService(IBaseRepository<Context> contextRepository)
{
    public async Task Create(ContextCreateDto data)
    {
        var context = new Context
        {
            MessageId = data.MessageId,
            
            MessageText = data.MessageText,
            MessageDate = data.MessageDate,
        };

        await contextRepository.AddAsync(context);
    }

    public async Task<Context?> Get(long id)
    {
        var result = await contextRepository.GetById(id);
        return result;
    }
}