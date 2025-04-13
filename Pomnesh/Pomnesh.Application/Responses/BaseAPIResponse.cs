namespace Pomnesh.API.Responses;

public class BaseApiResponse<T>
{
    public T? Payload { get; set; }
}