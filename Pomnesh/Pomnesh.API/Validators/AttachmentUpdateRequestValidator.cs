using FluentValidation;
using Pomnesh.API.Models;

namespace Pomnesh.API.Validators;

public class AttachmentUpdateRequestValidator : AbstractValidator<AttachmentUpdateRequest>
{
    public AttachmentUpdateRequestValidator()
    {
        // Id is required and positive
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id is required for update.");

        // Only validate fields if they were provided (not null)
        When(x => x.FileId.HasValue, () =>
            RuleFor(x => x.FileId.Value)
                .GreaterThan(0).WithMessage("FileId must be positive."));

        When(x => x.OwnerId.HasValue, () =>
            RuleFor(x => x.OwnerId.Value)
                .GreaterThan(0).WithMessage("OwnerId must be positive."));

        When(x => x.ContextId.HasValue, () =>
            RuleFor(x => x.ContextId.Value)
                .GreaterThan(0).WithMessage("ContextId must be positive."));

        When(x => x.OriginalLink != null, () =>
            RuleFor(x => x.OriginalLink)
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("OriginalLink must be a valid absolute URL."));
        
        RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Type must be a valid attachment type.");
    }
}