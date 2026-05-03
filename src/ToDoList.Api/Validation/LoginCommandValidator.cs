using FluentValidation;
using ToDoList.Contracts.Auth.Commands.Login;

namespace ToDoList.Api.Validation;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Password).NotEmpty();
    }
}
