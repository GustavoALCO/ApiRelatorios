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
        return await _context.Fiscais.FirstOrDefaultAsync(x => x.Login.ToUpper()
                                                                       .Contains(nome.ToUpper()));
    }

    public async Task<List<Dommain.Entities.User>> BuscarTodosFiscalLogin(string login)
    {
        return await _context.Fiscais.Where(x => x.Login.ToUpper()
                                                                .Contains(login.ToUpper()))
                                                                .ToListAsync();
    }

    public async Task<Dommain.Entities.User> BuscarFiscalLogin(string login)
    {
        return await _context.Fiscais.FirstOrDefaultAsync(x => x.Login == login);
    }

    public async Task<ICollection<Dommain.Entities.User>> BuscarListaFiscalIds(ICollection<int> idFiscal)
    {
        return await _context.Fiscais.
                            Where(x => idFiscal.Contains(x.UserId))
                            .ToListAsync();
    }

    public async Task<Dommain.Entities.User> BuscarFiscalId(int idFiscal)
    {
        return await _context.Fiscais.
                            FirstOrDefaultAsync(x => x.UserId == idFiscal);
    }

    public async Task<ICollection<Dommain.Entities.User>> BuscarTodosFiscais()
    {
        return await _context.Fiscais.ToListAsync();
    }
}
