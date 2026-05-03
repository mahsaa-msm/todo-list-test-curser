namespace ToDoList.Application.Contracts.Todos;

public sealed record CreateTodoResponse(int Id, string Title, bool IsCompleted, DateTime CreatedAt);
