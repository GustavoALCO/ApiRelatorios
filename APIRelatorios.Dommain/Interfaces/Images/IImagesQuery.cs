using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Dommain.Interfaces.Images;

public interface IImagesQuery
{
    Task<List<ImageData>> GetImageEvidencia(Guid evidenciaRotaId);
}
