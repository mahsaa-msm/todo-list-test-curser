using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Common;
using ToDoList.Contracts.Todos.Commands.UpdateStatus;

namespace ToDoList.Application.Todos.Commands.UpdateStatus;

public sealed class UpdateTodoStatusCommandHandler(IAppDbContext db, ICurrentUserService currentUser)
    : IRequestHandler<UpdateTodoStatusCommand>
{
    public async Task Handle(UpdateTodoStatusCommand request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId;
        var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == request.TodoId, cancellationToken);

        if (todo is null)
            throw new NotFoundAppException("Todo was not found.");

        if (todo.UserId != uid)
            throw new ForbiddenAppException("You cannot update this todo.");

        todo.SetCompletion(request.IsCompleted);
        await db.SaveChangesAsync(cancellationToken);
    }
}
