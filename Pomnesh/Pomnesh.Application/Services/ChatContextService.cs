using Pomnesh.API.Dto;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Interfaces;
using Pomnesh.Domain.Entity;
using Pomnesh.Application.Exceptions;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class ChatContextService(IBaseRepository<ChatContext> contextRepository) : IChatContextService
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

    public async Task<ChatContextResponse?> Get(long id)
    {
        var result = await contextRepository.GetById(id);
        if (result == null)
            throw new ContextNotFoundError(id);
        
        return new ChatContextResponse
        {
            Id = result.Id,
            MessageId = result.MessageId,
            MessageText = result.MessageText
        };
    }
    
    public async Task<IEnumerable<ChatContextResponse>> GetAll()
    {
        var result = await contextRepository.GetAll();
        
        var chatContextResponse = new List<ChatContextResponse>();
        foreach (var chatContext in result)
        {
            var responseDto = new ChatContextResponse
            {
                Id = chatContext.Id,
                MessageId = chatContext.MessageId,
                MessageText = chatContext.MessageText,
                MessageDate = chatContext.MessageDate
            };
            chatContextResponse.Add(responseDto);
        }

        return chatContextResponse;
    }

    public async Task Update(ChatContextUpdateDto data)
    {
        var chatContext = await contextRepository.GetById(data.Id);
        if (chatContext == null)
            throw new ContextNotFoundError(data.Id);

        var updatedChatContext = new ChatContext
        {
            Id = data.Id,
            MessageId = data.MessageId,
            MessageText = data.MessageText,
            MessageDate = data.MessageDate,
        };
        await contextRepository.Update(updatedChatContext);
    }

    public async Task Delete(long id)
    {
        var chatContext = await contextRepository.GetById(id);
        if (chatContext == null)
            throw new ContextNotFoundError(id);
        
        await contextRepository.Delete(id);
    }
}