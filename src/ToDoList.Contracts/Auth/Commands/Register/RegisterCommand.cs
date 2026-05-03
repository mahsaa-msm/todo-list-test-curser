using MediatR;

namespace ToDoList.Contracts.Auth.Commands.Register;

public sealed record RegisterCommand(string Username, string Password) : IRequest<int>;
