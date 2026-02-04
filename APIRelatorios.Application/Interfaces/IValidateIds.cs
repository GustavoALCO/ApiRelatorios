namespace APIRelatorios.Application.Interfaces;

public interface IValidateIds
{
    Task<bool> UserExisteAsync(int id);
    Task<bool> RotaExisteAsync(int id);
    Task<bool> EvidenciaExisteAsync(int id);
}
