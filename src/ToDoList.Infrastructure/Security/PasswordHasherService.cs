using Microsoft.AspNetCore.Identity;
using ToDoList.Application.Abstractions;

namespace ToDoList.Infrastructure.Security;

public sealed class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    private static readonly object Sentinel = new();

    public string Hash(string plainPassword) =>
        _hasher.HashPassword(Sentinel, plainPassword);

    public bool Verify(string plainPassword, string passwordHash) =>
        _hasher.VerifyHashedPassword(Sentinel, passwordHash, plainPassword) ==
        PasswordVerificationResult.Success;
}
