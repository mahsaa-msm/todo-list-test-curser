using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Contracts.Auth;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Auth.Commands;

public sealed class RegisterCommandHandler(IAppDbContext dbContext, IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterCommand, RegisterOutcome>
{
    public async Task<RegisterOutcome> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var username = command.Request.Username.Trim();
        var exists = await dbContext.Users.AnyAsync(x => x.Username == username, cancellationToken);
        if (exists) return new UsernameAlreadyRegistered();

        var user = new User { Username = username, PasswordHash = passwordHasher.Hash(command.Request.Password) };
        await dbContext.AddUserAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterSucceeded(user.Id, user.Username);
    }
}
