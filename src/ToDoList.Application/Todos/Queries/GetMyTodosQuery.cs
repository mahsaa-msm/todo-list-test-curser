using MediatR;
using ToDoList.Application.Contracts.Todos;

namespace ToDoList.Application.Todos.Queries;

public sealed record GetMyTodosQuery : IRequest<IReadOnlyList<TodoListItemDto>>;
