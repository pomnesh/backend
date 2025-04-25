namespace Pomnesh.Application.Models;

public class RecollectionCreateRequest
{
    public long UserId { get; set; }

    public required string DownloadLink { get; set; }
}