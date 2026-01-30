using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Dommain.Interfaces.Images;

public interface IEvidenciaRotaQuery
{
    Task<ICollection<EvidenciaRota>> GetImagemAsync(int RodaID);

    Task<EvidenciaRota> GetImageId(int imageId);
}
