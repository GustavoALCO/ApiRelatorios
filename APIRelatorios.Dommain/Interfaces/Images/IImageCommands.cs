using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Dommain.Interfaces.Images;

public interface IImageCommands
{
    Task SaveImage(Imagem img);

    Task DeleteImage(Imagem img);
}
