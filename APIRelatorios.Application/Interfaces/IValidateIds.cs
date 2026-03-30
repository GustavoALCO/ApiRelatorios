namespace APIRelatorios.Application.Interfaces;

public interface IValidateIds
{
    Task<bool> UserExisteAsync(int id);
    Task<bool> RotaExisteAsync(Guid id);
    Task<bool> EvidenciaExisteAsync(Guid id);
}
