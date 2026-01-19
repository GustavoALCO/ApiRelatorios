namespace APIRelatorios.Dommain.Interfaces.Rota;

public interface IRotaCommands
{
    Task CreateRotaAsync(Entities.Rota rota);

    Task DeleteRotaAsync(Entities.Rota rota);

    Task UpdateRotaAsync(Entities.Rota rota);
}
