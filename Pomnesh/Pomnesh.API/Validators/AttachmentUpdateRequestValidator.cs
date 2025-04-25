using FluentValidation;
using Pomnesh.API.Models;
using Pomnesh.Application.Models;

namespace Pomnesh.API.Validators;

public class AttachmentUpdateRequestValidator : AbstractValidator<AttachmentUpdateRequest>
{
    public AttachmentUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id is required for update.");

        RuleFor(x => x.FileId)
            .GreaterThan(0).WithMessage("FileId must be a positive number.");

        RuleFor(x => x.OwnerId)
            .GreaterThan(0).WithMessage("OwnerId must be a positive number.");

        RuleFor(x => x.ContextId)
            .GreaterThan(0).WithMessage("ContextId must be a positive number.");

        // OriginalLink is optional, but if provided, must be a valid URL
        RuleFor(x => x.OriginalLink)
            .Must(uri => 
                string.IsNullOrEmpty(uri) 
                || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("OriginalLink must be a valid absolute URL when provided.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be a valid attachment type.");
    }
}