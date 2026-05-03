using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Application.Abstractions;
using ToDoList.Infrastructure.Options;

namespace ToDoList.Infrastructure.Security;

public sealed class JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
    : IJwtTokenGenerator
{
    private readonly JwtOptions _jwt = jwtOptions.Value;

    public string CreateAccessToken(int userId, string username)
    {
        if (string.IsNullOrWhiteSpace(_jwt.Key))
            throw new InvalidOperationException("Jwt:Key is not configured.");

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credentials =
            new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expiry = DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            expires: expiry,
            signingCredentials: credentials,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
            ]);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
