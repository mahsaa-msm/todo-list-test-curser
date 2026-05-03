using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Application.Contracts.Auth;

namespace ToDoList.Application.Auth.Commands;

public sealed class LoginCommandHandler(
    IAppDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginCommand, LoginOutcome>
{
    public async Task<LoginOutcome> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(
            x => x.Username == command.Request.Username.Trim(),
            cancellationToken);

        if (user is null || !passwordHasher.Verify(command.Request.Password, user.PasswordHash))
        {
            return new InvalidCredentials();
        }

        var token = jwtTokenGenerator.GenerateToken(user);
        return new LoginSucceeded(token);
    }
}
