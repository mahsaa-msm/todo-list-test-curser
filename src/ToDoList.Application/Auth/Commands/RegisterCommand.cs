using MediatR;
using ToDoList.Application.Contracts.Auth;

namespace ToDoList.Application.Auth.Commands;

public sealed record RegisterCommand(RegisterRequest Request) : IRequest<RegisterOutcome>;
