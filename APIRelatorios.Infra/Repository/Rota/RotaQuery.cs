using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
using Azure;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;

namespace APIRelatorios.Infra.Repository.Rota;

public class RotaQuery : IRotaQuery
{
    private readonly DatabaseContext _context;

    public RotaQuery(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Dommain.Entities.Rota> BuscarRotaID(Guid id)
    {
        
            var image = await _context.Rota.FirstOrDefaultAsync(x => x.RotaId == id);

            return image;
    }

    public async Task<ICollection<Dommain.Entities.Rota>> BuscarRotasPorFiscais(ICollection<Dommain.Entities.User> Fiscais)
    {
        var idFiscais = Fiscais.Select(x => x.UserId).ToList();

        var rotas = await _context.Rota.Where(x => x.Fiscais.Any(f => idFiscais.Contains(f.UserId))).ToListAsync();

        return rotas;
    }

    public async Task<ICollection<Dommain.Entities.Rota>> BuscarRotasPorFiscal(int Fiscais, int page, int pageSize)
    {
        var rotas = await _context.Rota.Where(x => x.Fiscais.Any(f => f.UserId == Fiscais))
                                            .OrderByDescending(x => x.DataInicio)
                                            .Skip((page - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

        return rotas;
    }

    public async Task<ICollection<Dommain.Entities.Rota>> BuscarTodasRotas()
    {
        return await _context.Rota.ToListAsync();
    }

    public async Task<string> BuscarAlimentador(Guid id)
    {
        var rota =  await _context.Rota.FirstOrDefaultAsync(x => x.RotaId == id);

        return rota.Alimentador;
    }

    public async Task<ICollection<Dommain.Entities.Rota>> BuscarRotaFiltros(IQueryable<Dommain.Entities.Rota> rotas, int page, int pagesize)
    {
        var rota = await rotas.OrderByDescending(x => x.DataInicio).Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();

        return rota;
    }

    public async Task <ICollection<Dommain.Entities.Rota>> GetRotaFinish(Guid iduser,int page,int pagesize)
    {
        var rotas = await _context.Rota.Where(x => x.Fiscais.Any
                                                                (f => iduser.Equals(f.UserId))
                                                                && x.DataFinal != null)
                                                                .OrderByDescending(x => x.DataFinal)
                                                                .Skip((page - 1) * pagesize)
                                                                .Take(pagesize)
                                                                .ToListAsync();

        return rotas;
    }

    public IQueryable<Dommain.Entities.Rota> BuscarQuery()
    {
        return _context.Rota.AsQueryable();
    }
}
