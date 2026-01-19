namespace APIRelatorios.Dommain.Interfaces.User;

public interface IUserQuery
{
    Task<ICollection<Entities.User>> BuscarTodosFiscais();

    Task<Entities.User> BuscarFiscalNome(string nome);
}
