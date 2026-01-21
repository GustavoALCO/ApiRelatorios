using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Dommain.Interfaces.Images;

public interface IImageQuery
{
    Task<ICollection<Imagem>> GetImagemAsync(int RodaId);

    Task<Imagem> GetImageId(int imageId);
}
