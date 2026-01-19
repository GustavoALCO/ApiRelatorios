using APIRelatorios.Dommain.Interfaces.User;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Exeptions;
using Microsoft.EntityFrameworkCore;

namespace APIRelatorios.Infra.Repository.User;

public class UserCommands : IUserCommands
{
    private readonly DatabaseContext _context;

    public UserCommands(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateUser(Dommain.Entities.User user)
    {
        try
        {
            _context.Fiscais.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException(
                "Não foi possível criar o usuário no banco de dados.",
                ex);
        }
    }

    public async Task UpdateUser(Dommain.Entities.User user)
    {
        try
        {
            _context.Fiscais.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException(
                "Não foi possível atualizar o usuário no banco de dados.",
                ex);
        }
    }

    public async Task DeleteUser(Dommain.Entities.User user)
    {
        try
        {
            _context.Fiscais.Remove(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException(
                "Não foi possível remover o usuário do banco de dados.",
                ex);
        }
    }
}
