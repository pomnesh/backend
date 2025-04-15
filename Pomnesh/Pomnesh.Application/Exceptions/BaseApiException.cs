namespace Pomnesh.Application.Exceptions;

public abstract class BaseApiException : Exception
{
    public abstract int StatusCode { get; set; }
    public abstract string Description { get; set; }
    
    protected BaseApiException() : base() { }
}