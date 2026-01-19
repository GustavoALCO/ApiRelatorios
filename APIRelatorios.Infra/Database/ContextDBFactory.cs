using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace APIRelatorios.Infra.Database;

public class ContextDBFactory 
    : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=RelatorioDB;Username=AdminTeste;Password=Teste123");

        return new DatabaseContext(optionsBuilder.Options);
    }
}
