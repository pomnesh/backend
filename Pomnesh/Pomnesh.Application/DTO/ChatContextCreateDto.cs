namespace Pomnesh.Application.Dto;

public class ChatContextCreateDto
{
    public long MessageId { get; set; }
    
    public string? MessageText { get; set; }
    
    public DateTime MessageDate { get; set; }
}