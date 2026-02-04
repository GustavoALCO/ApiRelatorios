using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace APIRelatorios.Infra.Repository.Rota;

public class RotaQuery : IRotaQuery
{
    private readonly DatabaseContext _context;

    public RotaQuery(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Dommain.Entities.Rota> BuscarRotaID(int id)
    {
        
            var image = await _context.Rota.FirstOrDefaultAsync(x => x.RotaId == id);

            return image;
    }

    public async Task<ICollection<Dommain.Entities.Rota>> BuscarRotasPorFiscais(ICollection<Dommain.Entities.User> Fiscais)
    {
        var idFiscais = Fiscais.Select(x => x.UserId).ToList();

        var rotas = await _context.Rota.Where(x => x.Fiscais.Any(f => idFiscais.Contains(f.UserID))).ToListAsync();

        return rotas;
    }

    public async Task<ICollection<Dommain.Entities.Rota>> BuscarRotasPorFiscal(int Fiscais, int page, int pageSize)
    {
        var rotas = await _context.Rota.Where(x => x.Fiscais.Any(f => f.UserID == Fiscais))
                                            .Skip((page - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

        return rotas;
    }

    public async Task<ICollection<Dommain.Entities.Rota>> BuscarTodasRotas()
    {
        return await _context.Rota.ToListAsync();
    }

    public async Task<string> BuscarAlimentador(int id)
    {
        var rota =  await _context.Rota.FirstOrDefaultAsync(x => x.RotaId == id);

        return rota.Alimentador;
    }
}
