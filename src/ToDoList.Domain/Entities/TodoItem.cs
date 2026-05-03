namespace ToDoList.Domain.Entities;

public sealed class TodoItem
{
    public const int TitleMaxLength = 200;

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public User? User { get; private set; }

    private TodoItem()
    {
    }

    public static TodoItem Create(int userId, string title, DateTime createdAtUtc)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        title = RequireValidTitle(title);

        return new TodoItem
        {
            UserId = userId,
            Title = title,
            IsCompleted = false,
            CreatedAt = createdAtUtc
        };
    }

    public void SetCompletionStatus(bool isCompleted) => IsCompleted = isCompleted;

    private static string RequireValidTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        title = title.Trim();
        if (title.Length > TitleMaxLength)
            throw new ArgumentException($"Title must be at most {TitleMaxLength} characters.", nameof(title));

        return title;
    }
}
