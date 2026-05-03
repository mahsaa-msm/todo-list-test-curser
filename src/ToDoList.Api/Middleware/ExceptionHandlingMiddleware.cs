using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToDoList.Application.Common;
using ToDoList.Contracts.Common;

namespace ToDoList.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = environment.IsDevelopment(),
        };
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (AppException ex)
        {
            await WriteResponse(httpContext, StatusFor(ex), Map(ex));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception.");
            await WriteResponse(
                httpContext,
                StatusCodes.Status500InternalServerError,
                new ApiErrorResponse
                {
                    Code = "INTERNAL_ERROR",
                    Message = "An unexpected error occurred.",
                    Details = _environment.IsDevelopment() ? ex.Message : null,
                });
        }
    }

    private static ApiErrorResponse Map(AppException ex) =>
        new()
        {
            Code = ex.Code,
            Message = ex.Message,
        };

    private static int StatusFor(AppException ex) =>
        ex switch
        {
            ConflictAppException => StatusCodes.Status409Conflict,
            NotFoundAppException => StatusCodes.Status404NotFound,
            ForbiddenAppException => StatusCodes.Status403Forbidden,
            UnauthorizedAppException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

    private Task WriteResponse(HttpContext httpContext, int statusCode, ApiErrorResponse body)
    {
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        return JsonSerializer.SerializeAsync(httpContext.Response.Body, body, _jsonOptions);
    }
}
