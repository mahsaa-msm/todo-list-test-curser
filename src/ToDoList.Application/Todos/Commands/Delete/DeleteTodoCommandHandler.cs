using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Common;
using ToDoList.Contracts.Todos.Commands.Delete;

namespace ToDoList.Application.Todos.Commands.Delete;

public sealed class DeleteTodoCommandHandler(IAppDbContext db, ICurrentUserService currentUser)
    : IRequestHandler<DeleteTodoCommand>
{
    public async Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId;
        var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == request.TodoId, cancellationToken);

        if (todo is null)
            throw new NotFoundAppException("Todo was not found.");

        if (todo.UserId != uid)
            throw new ForbiddenAppException("You cannot delete this todo.");

        db.Todos.Remove(todo);
        await db.SaveChangesAsync(cancellationToken);
    }
}
