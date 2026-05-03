using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Abstractions;
using ToDoList.Domain.Common.Entities;
using ToDoList.Domain.Todos.Entities;
using ToDoList.Domain.Users.Entities;

namespace ToDoList.Infrastructure.Persistence;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IAuditUserProvider auditUserProvider)
    : DbContext(options), IAppDbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Todo> Todos => Set<Todo>();

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditing();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditing();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private void ApplyAuditing()
    {
        var now = DateTime.UtcNow;
        var userId = auditUserProvider.GetExecutingUserId();

        foreach (var entry in ChangeTracker.Entries<BaseEntity<long>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.UpdatedAt = null;
                    entry.Entity.UpdatedBy = null;
                    break;

                case EntityState.Modified:
                    entry.Property(nameof(BaseEntity<long>.CreatedAt)).IsModified = false;
                    entry.Property(nameof(BaseEntity<long>.CreatedBy)).IsModified = false;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userId;
                    break;
            }
        }
    }
}
