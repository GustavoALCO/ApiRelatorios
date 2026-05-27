using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
using Microsoft.EntityFrameworkCore;

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
        try
        {
            var image = await _Context.EvidenciaRota.Include(x => x.CheckList).FirstOrDefaultAsync(i => i.EvidenciaRotaId == imageId);

        return image;
        }
        catch (InvalidOperationException ex)
        {
            throw new RepositoryException("Erro ao buscar informações da imagem no banco de dados.",
                ex);
        }
    }

    public async Task<List<EvidenciaRota>> GetEvidenciaAsync(Guid RotaID)
    {
        try
        {
            var image = await _Context.EvidenciaRota.Include(x => x.CheckList).Where(i => i.RotaId == RotaID)
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

    public async Task<ICollection<EvidenciaRota>> GetEvidenciasPagination(Guid RotaID, int page, int pagesize)
    {
        try
        {
            var image = await _Context.EvidenciaRota.Include(x => x.CheckList).Include(x => x.Images).Where(i => i.RotaId == RotaID)
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

    public async Task<ICollection<EvidenciaRota>> GetEvidenciasUrgencia(Guid RotaID)
    {
        try
        {
            var image = await _Context.EvidenciaRota
                .Include(x => x.CheckList)
                .Include(x => x.Images)
                .Where(i => i.RotaId == RotaID && i.Emergencial == true)
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
}
