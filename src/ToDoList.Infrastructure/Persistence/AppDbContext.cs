using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Common.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<User> UsersSet => Set<User>();
    public DbSet<TodoItem> TodosSet => Set<TodoItem>();
    public IQueryable<User> Users => UsersSet.AsQueryable();
    public IQueryable<TodoItem> Todos => TodosSet.AsQueryable();

    public Task AddUserAsync(User user, CancellationToken cancellationToken = default) =>
        UsersSet.AddAsync(user, cancellationToken).AsTask();

    public Task AddTodoAsync(TodoItem todoItem, CancellationToken cancellationToken = default) =>
        TodosSet.AddAsync(todoItem, cancellationToken).AsTask();

    public void RemoveTodo(TodoItem todoItem) => TodosSet.Remove(todoItem);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Username).HasMaxLength(100).IsRequired();
            entity.Property(x => x.PasswordHash).IsRequired();
            entity.HasIndex(x => x.Username).IsUnique();
        });

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.IsCompleted).HasDefaultValue(false).IsRequired();
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();
            entity.HasOne(x => x.User).WithMany(x => x.Todos).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
