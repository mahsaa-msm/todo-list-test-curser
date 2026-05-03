using MediatR;

namespace ToDoList.Contracts.Todos.Commands.Delete;

public sealed record DeleteTodoCommand(long TodoId) : IRequest;
