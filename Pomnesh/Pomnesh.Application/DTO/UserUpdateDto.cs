namespace Pomnesh.Application.DTO;

public class UserUpdateDto
{
    public long Id { get; set; }

    public long VkId { get; set; }

    public string? VkToken { get; set; }
}