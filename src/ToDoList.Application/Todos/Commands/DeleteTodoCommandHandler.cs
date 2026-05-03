using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Contracts.Todos;

namespace ToDoList.Application.Todos.Commands;

public sealed class DeleteTodoCommandHandler(IAppDbContext dbContext, ICurrentUserService currentUserService)
    : IRequestHandler<DeleteTodoCommand, TodoMutationResult>
{
    public async Task<TodoMutationResult> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User token is missing.");

        var todo = await dbContext.Todos.FirstOrDefaultAsync(x => x.Id == command.TodoId, cancellationToken);
        if (todo is null) return TodoMutationResult.NotFound;
        if (todo.UserId != userId) return TodoMutationResult.Forbidden;

        dbContext.RemoveTodo(todo);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TodoMutationResult.Succeeded;
    }
}
