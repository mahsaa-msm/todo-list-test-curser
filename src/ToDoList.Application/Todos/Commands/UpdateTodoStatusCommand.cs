using MediatR;
using ToDoList.Application.Contracts.Todos;

namespace ToDoList.Application.Todos.Commands;

public sealed record UpdateTodoStatusCommand(int TodoId, UpdateTodoStatusRequest Request) : IRequest<TodoMutationResult>;
