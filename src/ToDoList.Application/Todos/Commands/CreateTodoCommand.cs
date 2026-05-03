using MediatR;
using ToDoList.Application.Contracts.Todos;

namespace ToDoList.Application.Todos.Commands;

public sealed record CreateTodoCommand(CreateTodoRequest Request) : IRequest<CreateTodoResponse>;
