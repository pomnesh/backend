using Pomnesh.API.Dto;
using Pomnesh.Application.Interfaces;
using Pomnesh.Domain.Entity;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Models;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Application.Services;

public class ChatContextService(IBaseRepository<ChatContext> contextRepository) : IChatContextService
{
    public async Task<int> Create(ChatContextCreateRequest request)
    {
        var context = new ChatContext
        {
            MessageId = request.MessageId,
            MessageText = request.MessageText,
            MessageDate = request.MessageDate,
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

    public async Task Update(ChatContextUpdateRequest request)
    {
        var chatContext = await contextRepository.GetById(request.Id);
        if (chatContext == null)
            throw new ContextNotFoundError(request.Id);

        var updatedChatContext = new ChatContext
        {
            Id = request.Id,
            MessageId = request.MessageId,
            MessageText = request.MessageText,
            MessageDate = request.MessageDate,
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