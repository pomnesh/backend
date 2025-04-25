using FluentValidation;
using Pomnesh.Application.Models;

namespace Pomnesh.API.Validators;

public class RecollectionCreateRequestValidator : AbstractValidator<RecollectionCreateRequest>
{
    public RecollectionCreateRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("UserId is required for update.");

        RuleFor(x => x.DownloadLink)
            .NotNull().WithMessage("DownloadLink must be provided.");
    }
}