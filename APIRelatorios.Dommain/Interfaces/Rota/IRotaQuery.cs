namespace APIRelatorios.Dommain.Interfaces.Rota;

public interface IRotaQuery
{
    Task<Entities.Rota> BuscarRotaID(Guid id);

    Task<ICollection<Entities.Rota>> BuscarTodasRotas();

    Task<ICollection<Entities.Rota>> BuscarRotasPorFiscais(ICollection<Entities.User> Fiscais);

    Task<ICollection<Entities.Rota>> BuscarRotasPorFiscal(int Fiscais, int page, int pageSize);

    Task<string> BuscarAlimentador(Guid id);

    IQueryable<Dommain.Entities.Rota> BuscarQuery();

    Task<ICollection<Dommain.Entities.Rota>> GetAmostra();

    Task<ICollection<Dommain.Entities.Rota>> BuscarRotaFiltros(IQueryable<Dommain.Entities.Rota> rotas, int page, int pagesize);
}
