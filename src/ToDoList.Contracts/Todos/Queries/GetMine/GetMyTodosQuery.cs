using MediatR;

namespace ToDoList.Contracts.Todos.Queries.GetMine;

public sealed record GetMyTodosQuery : IRequest<List<TodoDto>>;
