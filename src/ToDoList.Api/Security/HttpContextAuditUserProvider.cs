using System.Security.Claims;
using ToDoList.Application.Abstractions;

namespace ToDoList.Api.Security;

public sealed class HttpContextAuditUserProvider(IHttpContextAccessor accessor) : IAuditUserProvider
{
    public long? GetExecutingUserId()
    {
        var raw = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return long.TryParse(raw, out var id) ? id : null;
    }
}
