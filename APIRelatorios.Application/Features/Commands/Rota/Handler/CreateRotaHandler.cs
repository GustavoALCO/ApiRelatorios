using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRotaHandler
{

    private readonly IRotaCommands _rotaCommands;

    private readonly IValidateIds _validateIds;

    public CreateRotaHandler(IRotaCommands rotaCommands, IUserQuery userQuery, IValidateIds validateIds)
    {
        _rotaCommands = rotaCommands;
        _validateIds = validateIds;
    }

    public async Task Handler(CreateRotaCommand _commands)
    {
        foreach(var fiscais in _commands.Fiscais)
        {
            if (await _validateIds.UserExisteAsync(fiscais) is false)
                throw new Exception("Lista de usuarios invalidas");
        }

        Dommain.Entities.Rota rota = new(_commands.NomeRota,
                                        _commands.Alimentador,
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
