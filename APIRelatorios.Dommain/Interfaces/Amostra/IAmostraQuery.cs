namespace APIRelatorios.Domain.Interfaces.Amostra;

public interface IAmostraQuery
{
    Task<List<Domain.Entities.Amostra>> GetAmostraById(Guid idRota);

    Task<Domain.Entities.Amostra?> GetAmostraId(int id);
}
