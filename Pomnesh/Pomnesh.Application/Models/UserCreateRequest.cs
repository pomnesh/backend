namespace Pomnesh.Application.Models;

public class UserCreateRequest
{
    public long VkId { get; set; }

    public string? VkToken { get; set; }
}