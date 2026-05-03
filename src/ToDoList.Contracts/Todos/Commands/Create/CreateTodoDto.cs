namespace ToDoList.Contracts.Todos.Commands.Create;

/// <summary>Response payload after a todo is created (e.g. 201 Created body).</summary>
public sealed class CreateTodoDto
{
    public int Id { get; set; }
}
