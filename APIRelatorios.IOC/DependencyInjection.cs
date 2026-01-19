using APIRelatorios.Application.Settings;
using APIRelatorios.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace APIRelatorios.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddDBConfig(
    this IServiceCollection services,
    IConfiguration configuration,
    DBSettings dbSettings)
    {
        

        services.AddDbContext<DatabaseContext>((serviceProvider, options) =>
        {
            

            var connectionString =
                $"Host={dbSettings.Host};" +
                $"Port={dbSettings.Port};" +
                $"Database={dbSettings.Database};" +
                $"Username={dbSettings.User};" +
                $"Password={dbSettings.Password}";

            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(
                        typeof(DatabaseContext).Assembly.FullName);
                });
        });

        return services;
    }


}
