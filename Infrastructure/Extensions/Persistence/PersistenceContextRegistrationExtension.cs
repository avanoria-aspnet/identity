using Infrastructure.Persistence.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Extensions.Persistence;

public static class PersistenceRegistrationExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);

        if (environment.IsDevelopment())
        {
            services.AddSingleton<SqliteConnection>(_ =>
            {
                var connection = new SqliteConnection("Data Source=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<PersistenceContext>((serviceProvider, options) =>
            {
                var connection = serviceProvider.GetRequiredService<SqliteConnection>();
                options.UseSqlite(connection);
            });
        }
        else
        {
            var connectionString = configuration.GetConnectionString("SqlServerConnection")
                ?? throw new InvalidOperationException("Connection string 'SqlServerConnection' was not found.");

            services.AddDbContext<PersistenceContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        return services;
    }
}
