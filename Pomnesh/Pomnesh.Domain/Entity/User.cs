namespace Pomnesh.Domain.Entity;

public class User
{
    public long Id { get; set; }

    public long VkId {get; set;}

    public string? VkToken { get; set; }
}