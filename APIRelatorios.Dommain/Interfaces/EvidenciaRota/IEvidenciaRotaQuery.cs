using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Dommain.Interfaces.Images;

public interface IEvidenciaRotaQuery
{
    Task<ICollection<EvidenciaRota>> GetEvidenciaAsync(Guid RotaID);

    Task<ICollection<EvidenciaRota>> GetEvidenciasUrgencia(Guid RotaID);

    Task<EvidenciaRota> GetEvidenciaId(Guid imageId);

    Task<ICollection<EvidenciaRota>> GetEvidenciasPagination(Guid RotaID, int page, int pagesize);
}
