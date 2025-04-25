namespace Pomnesh.Application.Models;

public class RecollectionUpdateRequest
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public required string DownloadLink { get; set; }   
}