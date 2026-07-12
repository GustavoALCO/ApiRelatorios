using APIRelatorios.Domain.Interfaces.Amostra;
using APIRelatorios.Infra.Database;

namespace APIRelatorios.Infra.Repository.Amostra;

public class AmostraCommands : IAmostraCommands
{

     readonly DatabaseContext _databaseContext;

    public AmostraCommands(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    // Salvar amostra no banco de dados
    public async Task SaveAmostra(Domain.Entities.Amostra amostra)
    {
        await _databaseContext.Amostras.AddAsync(amostra);

        await _databaseContext.SaveChangesAsync();
    }

    // Atualizar amostra no banco de dados
    public async Task UpdateAmostra(Domain.Entities.Amostra amostra)
    {
        _databaseContext.Amostras.Update(amostra);

        await _databaseContext.SaveChangesAsync();
    }

}
