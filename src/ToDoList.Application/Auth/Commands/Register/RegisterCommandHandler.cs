using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Common;
using ToDoList.Contracts.Auth.Commands.Register;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Auth.Commands.Register;

public sealed class RegisterCommandHandler(IAppDbContext db, IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterCommand, int>
{
    public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var trimmed = request.Username.Trim();
        if (await db.Users.AsNoTracking().AnyAsync(u => u.Username == trimmed, cancellationToken))
            throw new ConflictAppException("Username is already taken.");

        var hash = passwordHasher.Hash(request.Password);
        var user = User.Register(trimmed, hash);
        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}
