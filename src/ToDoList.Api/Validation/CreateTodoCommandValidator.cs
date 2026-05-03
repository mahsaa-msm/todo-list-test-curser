using FluentValidation;
using ToDoList.Contracts.Todos.Commands.Create;

namespace ToDoList.Api.Validation;

public sealed class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Must(t => !string.IsNullOrWhiteSpace(t))
            .WithMessage("Title is required.")
            .MaximumLength(500);
    }
}
