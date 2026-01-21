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

        var rota = new Dommain.Entities.Rota
        {
            NomeRota = _commands.NomeRota ?? "",
            DataInicio = DateTime.UtcNow,
        };

        foreach (var userId in _commands.Fiscais)
        {
            rota.AdicionarFiscal(userId);
        }

        await _rotaCommands.CreateRotaAsync(rota);
    }
}
