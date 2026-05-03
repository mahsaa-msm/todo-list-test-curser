using FluentValidation;
using ToDoList.Contracts.Auth.Commands.Register;

namespace ToDoList.Api.Validation;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}
