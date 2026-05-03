using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Todos.Entities;
using ToDoList.Domain.Users.Entities;

namespace ToDoList.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    DbSet<Todo> Todos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
