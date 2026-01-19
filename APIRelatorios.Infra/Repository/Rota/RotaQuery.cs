using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
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
        
            var image = await _context.Rota.FindAsync(id);

            if (image == null)
                throw new RepositoryException("Erro ao buscar informações da imagem no banco de dados.");

            return image;
    }

    public async Task<ICollection<Dommain.Entities.Rota>> BuscarRotasPorFiscais(ICollection<Dommain.Entities.User> Fiscais)
    {
        var idFiscais = Fiscais.Select(x => x.UserId).ToList();

        var rotas = await _context.Rota.Where(x => x.Fiscais.Any(f => idFiscais.Contains(f.UserID))).ToListAsync();

        return rotas;
    }

    public async Task<ICollection<Dommain.Entities.Rota>> BuscarTodasRotas()
    {
        return await _context.Rota.ToListAsync();
    }
}
