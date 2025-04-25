namespace Pomnesh.Application.Models;

public class ChatContextCreateRequest
{
    public long MessageId { get; set; }

    public string? MessageText { get; set; }

    public DateTime MessageDate { get; set; }
}