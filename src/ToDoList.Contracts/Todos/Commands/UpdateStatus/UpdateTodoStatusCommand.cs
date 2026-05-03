using MediatR;

namespace ToDoList.Contracts.Todos.Commands.UpdateStatus;

/// <summary>
/// PATCH body carries <see cref="IsCompleted"/> only; API merges route <c>id</c> via
/// <c>command with { TodoId = id }</c> before MediatR sends.
/// </summary>
public sealed record UpdateTodoStatusCommand : IRequest
{
    public long TodoId { get; init; }

    public bool IsCompleted { get; init; }
}
