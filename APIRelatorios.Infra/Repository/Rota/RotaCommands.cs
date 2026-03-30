using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace APIRelatorios.Infra.Repository.Rota;

public class RotaCommands : IRotaCommands
{
    private readonly DatabaseContext _context;

    public RotaCommands(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AdicionarFiscalRota(UsuarioRota usuarioRota)
    {
        try
        {
            await _context.AddAsync(usuarioRota);

            await _context.SaveChangesAsync();
        }
        catch (DbException ex)
        {
            throw new RepositoryException("Erro ao remover Usuario da Rota",
                ex);
        }
    }

    public async Task CreateRotaAsync(Dommain.Entities.Rota rota)
    {
        try
        {
            await _context.Rota.AddAsync(rota);

            await _context.SaveChangesAsync();
        }
        catch(DbException ex)
        {
            throw new RepositoryException("Erro ao adicionar Rota ao banco de dados",
                ex);
        }
    }

    public async Task DeleteRotaAsync(Dommain.Entities.Rota rota)
    {
        try
        {
             _context.Rota.Remove(rota);

            await _context.SaveChangesAsync();
        }
        catch (DbException ex)
        {
            throw new RepositoryException("Erro ao remover Rota ao banco de dados",
                ex);
        }
    }

    public async Task RemoverFiscalRota(int userId, Guid idrota)
    {
        try
        {

            var user = await _context.UsuarioRotas.FirstOrDefaultAsync(x => x.UserId == userId && x.RotaId == idrota);

            _context.Remove(user);

            await _context.SaveChangesAsync();
        }
        catch (DbException ex)
        {
            throw new RepositoryException("Erro ao remover Usuario da Rota",
                ex);
        }
    }

    public async Task UpdateRotaAsync(Dommain.Entities.Rota rota)
    {
        try
        {
            _context.Rota.Update(rota);

            await _context.SaveChangesAsync();
        }
        catch (DbException ex)
        {
            throw new RepositoryException("Erro ao remover Rota ao banco de dados",
                ex);
        }
    }
}
