using ToDoList.Domain.Common.Entities;
using ToDoList.Domain.Users.Entities;

namespace ToDoList.Domain.Todos.Entities;

public sealed class Todo : BaseEntity<long>
{
#pragma warning disable CS8618
    private Todo()
    {
        // EF Core
    }
#pragma warning restore CS8618

    public long UserId { get; private set; }

    public string Title { get; private set; } = "";

    public bool IsCompleted { get; private set; }

    public User User { get; private set; } = null!;

    public Todo(long userId, string title)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        UserId = userId;
        Title = title.Trim();
        IsCompleted = false;
    }

    public void SetCompletion(bool isCompleted) => IsCompleted = isCompleted;
}
