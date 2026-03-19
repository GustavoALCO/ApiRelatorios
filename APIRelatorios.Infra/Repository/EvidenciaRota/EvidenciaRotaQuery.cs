using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace APIRelatorios.Infra.Repository.Images;

public class EvidenciaRotaQuery : IEvidenciaRotaQuery
{
    private readonly DatabaseContext _Context;

    public EvidenciaRotaQuery(DatabaseContext context)
    {
        _Context = context;
    }

    public async Task<EvidenciaRota> GetEvidenciaId(Guid imageId)
    {
       
        var image = await _Context.EvidenciaRota.FirstOrDefaultAsync(i => i.EvidenciaRotaId == imageId);

        return image;
        
    }

    public async Task<ICollection<EvidenciaRota>> GetEvidenciaAsync(int RotaID)
    {
        try
        {
            var image = await _Context.EvidenciaRota.Where(i => i.RotaId == RotaID)
                .OrderBy(x => x.Horario)
                .ToListAsync();

            return image;
        }
        catch (InvalidOperationException ex)
        {
            throw new RepositoryException("Erro ao buscar informações da imagem no banco de dados.",
                ex);
        }
    }

    public async Task<ICollection<EvidenciaRota>> GetEvidenciasPagination(int RotaID, int page, int pagesize)
    {
        try
        {
            var image = await _Context.EvidenciaRota.Where(i => i.RotaId == RotaID)
                .OrderBy(x => x.Horario)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return image;
        }
        catch (InvalidOperationException ex)
        {
            throw new RepositoryException("Erro ao buscar informações da imagem no banco de dados.",
                ex);
        }
    }
}
