using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Common;
using ToDoList.Contracts.Auth.Commands.Login;

namespace ToDoList.Application.Auth.Commands.Login;

public sealed class LoginCommandHandler(IAppDbContext db, IPasswordHasher passwordHasher, IJwtTokenGenerator jwt)
    : IRequestHandler<LoginCommand, string>
{
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var trimmed = request.Username.Trim();
        var user = await db.Users.AsNoTracking()
            .SingleOrDefaultAsync(u => u.Username == trimmed, cancellationToken);

        if (user is null ||
            !passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAppException("Invalid username or password.");

        return jwt.CreateAccessToken(user.Id, user.Username);
    }
}
