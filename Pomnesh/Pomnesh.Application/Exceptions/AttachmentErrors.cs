namespace Pomnesh.Application.Exceptions;

public class AttachmentNotFoundError(long attachmentId) : BaseApiException
{
    public override int StatusCode
    {
        get => 404;
        set => throw new NotImplementedException();
    }

    public override string Description
    {
        get => $"The attachment with ID '{AttachmentId}' was not found";
        set => throw new NotImplementedException();
    }
    public long AttachmentId { get; } = attachmentId;
}