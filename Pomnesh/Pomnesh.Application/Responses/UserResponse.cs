namespace Pomnesh.API.Dto;

public class UserResponse
{
    public long Id { get; set; }

    public long VkId { get; set; }

    public string? VkToken { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}