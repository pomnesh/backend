namespace Pomnesh.Application.Exceptions;

public class UserNotFoundError(long userId) : BaseApiException
{
    public override int StatusCode
    {
        get => 404;
        set => throw new NotImplementedException();
    }

    public override string Description
    {
        get => $"The user with ID '{UserId}' was not found";
        set => throw new NotImplementedException();
    }
    public long UserId { get; } = userId;
}