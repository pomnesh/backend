namespace Pomnesh.Application.DTO;

public class RecollectionUpdateDto
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public required string DownloadLink { get; set; }
}