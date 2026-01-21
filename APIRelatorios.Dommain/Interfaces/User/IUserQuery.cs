namespace APIRelatorios.Dommain.Interfaces.User;

public interface IUserQuery
{
    Task<ICollection<Entities.User>> BuscarTodosFiscais();

    Task<Entities.User> BuscarFiscalNome(string nome);

    Task<ICollection<Entities.User>> BuscarListaFiscalIds(ICollection<int> idFiscal);

    Task<Dommain.Entities.User> BuscarListaFiscalId(int idFiscal);
}
