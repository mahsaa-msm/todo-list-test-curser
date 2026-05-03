namespace ToDoList.Contracts.Todos.Queries.GetMine;

/// <summary>Read model returned by listing the current user's todos.</summary>
public sealed record TodoDto(int Id, string Title, bool IsCompleted, DateTime CreatedAt);
