using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Dommain.Interfaces.Images;

public interface IEvidenciaRotaCommands
{
    Task SaveImage(EvidenciaRota img);

    Task DeleteImage(EvidenciaRota img);

    Task UpdateImageAsync(EvidenciaRota img);
}
