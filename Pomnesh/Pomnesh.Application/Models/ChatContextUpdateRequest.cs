namespace Pomnesh.Application.Models;

public class ChatContextUpdateRequest
{
    public long Id { get; set; }

    public long MessageId { get; set; }

    public string? MessageText { get; set; }

    public DateTime MessageDate { get; set; }
}