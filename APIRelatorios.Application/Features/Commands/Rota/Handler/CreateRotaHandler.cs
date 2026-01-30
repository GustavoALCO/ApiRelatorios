using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRotaHandler
{

    private readonly IRotaCommands _rotaCommands;

    public CreateRotaHandler(IRotaCommands rotaCommands, IUserQuery userQuery)
    {
        _rotaCommands = rotaCommands;
    }

    public async Task Handler(CreateRotaCommand _commands)
    {

        Dommain.Entities.Rota rota = new(_commands.NomeRota,
                                         DateTime.UtcNow);

        if (_commands.Fiscais == null || _commands.Fiscais.Count == 0)
            throw new Exception("Fiscais não podem ser nulos ou vazio");

        foreach (var userId in _commands.Fiscais)
        {
            UsuarioRota usuarioRota = new()
            {
            UserID = userId
            };

            rota.Fiscais.Add(usuarioRota);
        }

        await _rotaCommands.CreateRotaAsync(rota);
    }
}
