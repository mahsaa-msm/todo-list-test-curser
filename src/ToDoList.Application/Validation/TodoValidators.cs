using FluentValidation;
using ToDoList.Application.Contracts.Todos;

namespace ToDoList.Application.Validation;

public sealed class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}

public sealed class UpdateTodoStatusRequestValidator : AbstractValidator<UpdateTodoStatusRequest>
{
    public UpdateTodoStatusRequestValidator()
    {
        RuleFor(x => x).NotNull();
    }
}
