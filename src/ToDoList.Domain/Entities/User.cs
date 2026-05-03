namespace ToDoList.Domain.Entities;

public sealed class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public List<TodoItem> Todos { get; set; } = [];
}
