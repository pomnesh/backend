namespace Pomnesh.API.Dto;

public class UserResponse
{
    public long Id { get; set; }

    public long VkId { get; set; }

    public string? VkToken { get; set; }
}