namespace ToDoList.Application.Common;

public abstract class AppException : Exception
{
    protected AppException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    public string Code { get; }
}

public sealed class ConflictAppException(string message)
    : AppException("CONFLICT", message);

public sealed class NotFoundAppException(string message)
    : AppException("NOT_FOUND", message);

public sealed class ForbiddenAppException(string message)
    : AppException("FORBIDDEN", message);

public sealed class UnauthorizedAppException(string message)
    : AppException("UNAUTHORIZED", message);
