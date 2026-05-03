using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await WriteJsonAsync(context, new { message = "Validation failed", errors = ex.Errors.Select(x => x.ErrorMessage) });
        }
        catch (DbUpdateException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await WriteJsonAsync(context, new { message = "Database conflict", detail = ex.InnerException?.Message ?? ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await WriteJsonAsync(context, new { message = ex.Message });
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await WriteJsonAsync(context, new { message = "Unexpected server error" });
        }
    }

    private static Task WriteJsonAsync(HttpContext context, object payload)
    {
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
