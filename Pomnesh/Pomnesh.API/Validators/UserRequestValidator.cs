using FluentValidation;
using Pomnesh.Application.Models;

namespace Pomnesh.API.Validators;

public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
{
    public UserCreateRequestValidator()
    {
        RuleFor(x => x.VkId)
            .GreaterThan(0).WithMessage("VkId must be a positive number.");
    }
}

public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
{
    public UserUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id is required for update.");

        RuleFor(x => x.VkId)
            .GreaterThan(0).WithMessage("VkId must be a positive number.");
    }
}