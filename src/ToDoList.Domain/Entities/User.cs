namespace ToDoList.Domain.Entities;

public sealed class User
{
    public const int UsernameMaxLength = 100;

    public int Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    public List<TodoItem> Todos { get; private set; } = [];

    private User()
    {
    }

    public static User Create(string username, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        username = RequireValidUsername(username);

        return new User
        {
            Username = username,
            PasswordHash = passwordHash
        };
    }

    private static string RequireValidUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.", nameof(username));

        username = username.Trim();
        if (username.Length > UsernameMaxLength)
            throw new ArgumentException($"Username must be at most {UsernameMaxLength} characters.", nameof(username));

        return username;
    }
}
