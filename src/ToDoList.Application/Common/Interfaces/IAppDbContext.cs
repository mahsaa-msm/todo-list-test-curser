using ToDoList.Domain.Entities;

namespace ToDoList.Application.Common.Interfaces;

public interface IAppDbContext
{
    IQueryable<User> Users { get; }
    IQueryable<TodoItem> Todos { get; }
    Task AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task AddTodoAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
    void RemoveTodo(TodoItem todoItem);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
