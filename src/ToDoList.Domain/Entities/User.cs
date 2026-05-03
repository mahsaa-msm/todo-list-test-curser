namespace ToDoList.Domain.Entities;

public sealed class User
{
#pragma warning disable CS8618
    private User()
    {
        // EF Core
    }
#pragma warning restore CS8618

    public int Id { get; private set; }

    public string Username { get; private set; } = "";

    public string PasswordHash { get; private set; } = "";

    public ICollection<Todo> Todos { get; private set; } = new List<Todo>();

    public static User Register(string username, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        return new User
        {
            Username = username.Trim(),
            PasswordHash = passwordHash,
        };
    }
}
