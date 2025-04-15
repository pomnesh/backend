namespace Pomnesh.Application.Exceptions;

public class RecollectionNotFoundError(long recollectionId) : BaseApiException
{
    public override int StatusCode
    {
        get => 404;
        set => throw new NotImplementedException();
    }

    public override string Description
    {
        get => $"The recollection with ID '{RecollectionId}' was not found";
        set => throw new NotImplementedException();
    }
    public long RecollectionId { get; } = recollectionId;
}