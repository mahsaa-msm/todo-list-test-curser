namespace ToDoList.Application.Contracts.Todos;

public sealed record TodoListItemDto(int Id, string Title, bool IsCompleted, DateTime CreatedAt);
