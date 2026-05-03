namespace ToDoList.Domain.Common.Entities;

/// <summary>
/// Base for persistent entities: primary key and audit fields.
/// Audit values are set from infrastructure (e.g. SaveChanges), not from domain factories.
/// </summary>
public abstract class BaseEntity<TId>
    where TId : IEquatable<TId>
{
    public TId Id { get; internal set; } = default!;

    public long? CreatedBy { get; internal set; }

    public DateTime CreatedAt { get; internal set; }

    public long? UpdatedBy { get; internal set; }

    public DateTime? UpdatedAt { get; internal set; }
}
