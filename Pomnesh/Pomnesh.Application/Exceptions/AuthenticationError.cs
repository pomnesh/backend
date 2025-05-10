namespace Pomnesh.Application.Exceptions;

public class AuthenticationError : BaseApiException
{
    public override int StatusCode
    {
        get => 401;
        set { }
    }

    public override string Description { get; set; }

    public AuthenticationError(string message)
    {
        Description = message;
    }
} 