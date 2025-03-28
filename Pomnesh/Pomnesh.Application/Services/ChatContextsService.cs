using Pomnesh.Application.Dto;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class ChatContextsService(IBaseRepository<ChatContext> contextRepository)
{
    public async Task<int> Create(ChatContextCreateDto data)
    {
        var context = new ChatContext
        {
            MessageId = data.MessageId,
            MessageText = data.MessageText,
            MessageDate = data.MessageDate,
        };

        return await contextRepository.Add(context);
    }

    public async Task<ChatContext?> Get(long id)
    {
        var result = await contextRepository.GetById(id);
        return result;
    }
}