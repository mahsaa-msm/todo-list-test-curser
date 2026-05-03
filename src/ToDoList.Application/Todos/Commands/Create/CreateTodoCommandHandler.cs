using MediatR;
using ToDoList.Application.Abstractions;
using ToDoList.Contracts.Todos.Commands.Create;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Todos.Commands.Create;

public sealed class CreateTodoCommandHandler(IAppDbContext db, ICurrentUserService currentUser)
    : IRequestHandler<CreateTodoCommand, int>
{
    public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var todo = new Todo(userId, request.Title.Trim());
        db.Todos.Add(todo);
        await db.SaveChangesAsync(cancellationToken);
        return todo.Id;
    }
}
