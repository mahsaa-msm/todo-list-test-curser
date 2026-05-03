using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoList.Application.Abstractions;
using ToDoList.Infrastructure.Options;
using ToDoList.Infrastructure.Persistence;
using ToDoList.Infrastructure.Security;

namespace ToDoList.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        if (environment.IsEnvironment("Testing"))
        {
            var inMemoryDbName = Guid.NewGuid().ToString("N");
            services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase(inMemoryDbName));
        }
        else
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

            services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(connectionString));
        }

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddSingleton<IPasswordHasher, PasswordHasherService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
