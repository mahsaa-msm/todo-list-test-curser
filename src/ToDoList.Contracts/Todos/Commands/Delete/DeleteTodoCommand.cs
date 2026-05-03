using MediatR;

namespace ToDoList.Contracts.Todos.Commands.Delete;

public sealed record DeleteTodoCommand(int TodoId) : IRequest;
