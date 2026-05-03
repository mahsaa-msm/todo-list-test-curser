using MediatR;
using ToDoList.Application.Contracts.Auth;

namespace ToDoList.Application.Auth.Commands;

public sealed record LoginCommand(LoginRequest Request) : IRequest<LoginOutcome>;
