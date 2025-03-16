namespace Pomnesh.Application.Dto;

public class RecollectionCreateDto
{
    public long UserId { get; set; }
    
    public required string DownloadLink { get; set; }
}