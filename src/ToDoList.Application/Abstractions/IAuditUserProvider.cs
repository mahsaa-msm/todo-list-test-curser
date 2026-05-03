namespace ToDoList.Application.Abstractions;

/// <summary>
/// Optional user id for audit columns (CreatedBy/UpdatedBy). Null when no principal (e.g. anonymous register).
/// </summary>
public interface IAuditUserProvider
{
    long? GetExecutingUserId();
}
