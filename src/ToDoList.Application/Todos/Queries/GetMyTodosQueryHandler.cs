using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Contracts.Todos;

namespace ToDoList.Application.Todos.Queries;

public sealed class GetMyTodosQueryHandler(IAppDbContext dbContext, ICurrentUserService currentUserService)
    : IRequestHandler<GetMyTodosQuery, IReadOnlyList<TodoListItemDto>>
{
    public async Task<IReadOnlyList<TodoListItemDto>> Handle(GetMyTodosQuery _, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User token is missing.");

        var items = await dbContext.Todos
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TodoListItemDto(x.Id, x.Title, x.IsCompleted, x.CreatedAt))
            .ToListAsync(cancellationToken);

        return items;
    }
}
