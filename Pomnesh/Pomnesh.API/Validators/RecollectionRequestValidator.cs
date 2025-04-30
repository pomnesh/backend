using FluentValidation;
using Pomnesh.Application.Models;

namespace Pomnesh.API.Validators;

public class RecollectionCreateRequestValidator : AbstractValidator<RecollectionCreateRequest>
{
    public RecollectionCreateRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("UserId is required for update.")
            .GreaterThan(0).WithMessage("User ID must be a positive number.");

        RuleFor(x => x.DownloadLink)
            .NotNull().WithMessage("DownloadLink must be provided.")
            .NotEmpty().WithMessage("Download link is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("Download link must be a valid URL.");
    }
}

public class RecollectionUpdateRequestValidator : AbstractValidator<RecollectionUpdateRequest>
{
    public RecollectionUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID must be a positive number.");

        RuleFor(x => x.UserId)
            .NotNull().WithMessage("UserId is required for update.")
            .Must(x => x > 0).WithMessage("User ID must be a positive number.");

        RuleFor(x => x.DownloadLink)
            .NotNull().WithMessage("DownloadLink must be provided.")
            .NotEmpty().WithMessage("Download link is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("Download link must be a valid URL.");
    }
}