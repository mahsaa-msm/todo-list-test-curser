using System.Security.Claims;
using ToDoList.Application.Abstractions;

namespace ToDoList.Api.Security;

public sealed class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public int UserId =>
        int.TryParse(
            accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
            out var id)
            ? id
            : throw new InvalidOperationException("User is not authenticated.");
}
