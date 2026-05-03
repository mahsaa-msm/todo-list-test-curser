using MediatR;

namespace ToDoList.Contracts.Auth.Commands.Login;

public sealed record LoginCommand(string Username, string Password) : IRequest<string>;
