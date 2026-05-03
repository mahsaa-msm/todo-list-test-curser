namespace ToDoList.Domain.Entities;

public sealed class Todo
{
#pragma warning disable CS8618
    private Todo()
    {
        // EF Core
    }
#pragma warning restore CS8618

    public int Id { get; private set; }

    public int UserId { get; private set; }

    public string Title { get; private set; } = "";

    public bool IsCompleted { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public Todo(int userId, string title)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        UserId = userId;
        Title = title.Trim();
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetCompletion(bool isCompleted) => IsCompleted = isCompleted;
}
