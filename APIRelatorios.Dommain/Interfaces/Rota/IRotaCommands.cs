using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Dommain.Interfaces.Rota;

public interface IRotaCommands
{
    Task CreateRotaAsync(Entities.Rota rota);

    Task DeleteRotaAsync(Entities.Rota rota);

    Task UpdateRotaAsync(Entities.Rota rota);

    Task RemoverFiscalRota(int userId, Guid idrota);

    Task AdicionarFiscalRota(UsuarioRota usuarioRota);
}
