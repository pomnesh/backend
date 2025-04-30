using FluentValidation;
using Pomnesh.Application.Models;

namespace Pomnesh.API.Validators;

public class ChatContextCreateRequestValidator : AbstractValidator<ChatContextCreateRequest>
{
    public ChatContextCreateRequestValidator()
    {
        RuleFor(x => x.MessageId)
            .GreaterThan(0).WithMessage("MessageId must be a positive number.");

        RuleFor(x => x.MessageText)
            .NotNull().WithMessage("MessageText must be provided.");

        RuleFor(x => x.MessageDate)
            .Must(date => date != default(DateTime)).WithMessage("MessageDate must be provided.");
    }
}

public class ChatContextUpdateRequestValidator : AbstractValidator<ChatContextUpdateRequest>
{
    public ChatContextUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id is required for update.");

        RuleFor(x => x.MessageId)
            .GreaterThan(0).WithMessage("MessageId must be a positive number.");

        RuleFor(x => x.MessageText)
            .NotNull().WithMessage("MessageText must be provided.");

        RuleFor(x => x.MessageDate)
            .Must(date => date != default(DateTime)).WithMessage("MessageDate must be provided.");
    }
}