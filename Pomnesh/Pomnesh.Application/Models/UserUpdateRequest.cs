namespace Pomnesh.Application.Models;

public class UserUpdateRequest
{
    public long Id { get; set; }

    public long VkId { get; set; }

    public string? VkToken { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }
}