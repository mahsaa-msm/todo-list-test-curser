using MediatR;

namespace ToDoList.Contracts.Todos.Commands.Create;

public sealed record CreateTodoCommand(string Title) : IRequest<long>;
