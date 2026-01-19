namespace APIRelatorios.Dommain.Interfaces.Rota;

public interface IRotaQuery
{
    Task<Entities.Rota> BuscarRotaID(int id);

    Task<ICollection<Entities.Rota>> BuscarTodasRotas();

    Task<ICollection<Entities.Rota>> BuscarRotasPorFiscais(ICollection<Entities.User> Fiscais);
}
