using FluentValidation;
using Pomnesh.API.Models;

namespace Pomnesh.API.Validators;

public class AttachmentCreateRequestValidator : AbstractValidator<AttachmentCreateRequest>
{
    public AttachmentCreateRequestValidator()
    {
        RuleFor(x => x.FileId)
            .GreaterThan(0).WithMessage("FileId must be a positive number.");

        RuleFor(x => x.OwnerId)
            .GreaterThan(0).WithMessage("OwnerId must be a positive number.");

        RuleFor(x => x.ContextId)
            .NotNull().WithMessage("ContextId is required for create.");

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