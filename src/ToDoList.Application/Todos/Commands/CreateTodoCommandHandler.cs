using MediatR;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Contracts.Todos;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Todos.Commands;

public sealed class CreateTodoCommandHandler(IAppDbContext dbContext, ICurrentUserService currentUserService)
    : IRequestHandler<CreateTodoCommand, CreateTodoResponse>
{
    public async Task<CreateTodoResponse> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User token is missing.");

        var todo = TodoItem.Create(userId, command.Request.Title, DateTime.UtcNow);

        await dbContext.AddTodoAsync(todo, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTodoResponse(todo.Id, todo.Title, todo.IsCompleted, todo.CreatedAt);
    }
}
