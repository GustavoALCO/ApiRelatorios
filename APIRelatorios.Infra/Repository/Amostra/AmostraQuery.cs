using APIRelatorios.Domain.Interfaces.Amostra;
using APIRelatorios.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace APIRelatorios.Infra.Repository.Amostra;

public class AmostraQuery : IAmostraQuery
{
    private readonly DatabaseContext _databaseContext;

    public AmostraQuery(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<List<Domain.Entities.Amostra>> GetAmostraById(Guid idRota)
    {
        return await _databaseContext.Amostras.Where(a => a.RotaId == idRota).ToListAsync();
    }

    public async Task<List<Domain.Entities.Amostra>> GetAmostraCheck(Guid idRota)
    {
        return await _databaseContext.Amostras
            .Where(a => a.RotaId == idRota && a.Sincronizado)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Amostra> GetAmostraId(int id)
    {
        return await _databaseContext.Amostras.FindAsync(id);
    }
}
