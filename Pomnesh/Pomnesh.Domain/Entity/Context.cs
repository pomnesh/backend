namespace Pomnesh.Domain.Entity;

public class Context
{
    public long Id { get; set; }
    
    public long MessageId { get; set; }
    
    public string? MessageText { get; set; }
    
    public DateTime MessageDate { get; set; }
    
    // Optional: Navigation property for reverse relationship
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    
}