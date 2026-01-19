using APIRelatorios.Dommain.Interfaces.User;
using APIRelatorios.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace APIRelatorios.Infra.Repository.User;

public class UserQuery : IUserQuery
{
    private readonly DatabaseContext _context;

    public UserQuery(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Dommain.Entities.User> BuscarFiscalNome(string nome)
    {
        return await _context.Fiscais.FindAsync(nome);
    }

    public async Task<ICollection<Dommain.Entities.User>> BuscarTodosFiscais()
    {
        return await _context.Fiscais.ToListAsync();
    }
}
