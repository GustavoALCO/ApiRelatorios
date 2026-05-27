using APIRelatorios.Application.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace APIRelatorios.Infra.Database;

public class ContextDBFactory
    : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var environment =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");

        IConfigurationRoot configuration =
            new ConfigurationBuilder()
                .SetBasePath(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "../APIRelatorios.WebAPI"))
                .AddJsonFile(
                    "appsettings.json",
                    optional: false)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true)
                .AddEnvironmentVariables()
                .Build();

        var dbSettings =
            configuration
                .GetSection("ConnectionStrings")
                .Get<DBSettings>();

        var connectionString =
            $"Host={dbSettings!.Host};" +
            $"Port={dbSettings.Port};" +
            $"Database={dbSettings.Database};" +
            $"Username={dbSettings.User};" +
            $"Password={dbSettings.Password}";

        var optionsBuilder =
            new DbContextOptionsBuilder<DatabaseContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new DatabaseContext(optionsBuilder.Options);
    }
}