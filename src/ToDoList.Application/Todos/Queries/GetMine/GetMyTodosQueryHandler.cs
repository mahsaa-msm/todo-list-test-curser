using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Abstractions;
using ToDoList.Contracts.Todos.Queries.GetMine;

namespace ToDoList.Application.Todos.Queries.GetMine;

public sealed class GetMyTodosQueryHandler(IAppDbContext db, ICurrentUserService currentUser)
    : IRequestHandler<GetMyTodosQuery, List<TodoDto>>
{
    public async Task<List<TodoDto>> Handle(GetMyTodosQuery _, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId;

        var items = await db.Todos.AsNoTracking()
            .Where(t => t.UserId == uid)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TodoDto(t.Id, t.Title, t.IsCompleted, t.CreatedAt))
            .ToListAsync(cancellationToken);

        return items;
    }
}
