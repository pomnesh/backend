namespace Pomnesh.Application.Exceptions;

public class ContextNotFoundError(long contextId) : BaseApiException
{
    public override int StatusCode
    {
        get => 404;
        set => throw new NotImplementedException();
    }

    public override string Description
    {
        get => $"The context with ID '{ContextId}' was not found";
        set => throw new NotImplementedException();
    }
    public long ContextId { get; } = contextId;
}