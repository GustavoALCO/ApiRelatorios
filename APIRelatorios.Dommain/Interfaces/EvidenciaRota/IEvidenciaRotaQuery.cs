using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Dommain.Interfaces.Images;

public interface IEvidenciaRotaQuery
{
    Task<ICollection<EvidenciaRota>> GetEvidenciaAsync(int RodaID);

    Task<EvidenciaRota> GetEvidenciaId(int imageId);

    Task<ICollection<EvidenciaRota>> GetEvidenciasPagination(int RotaID, int page, int pagesize);
}
