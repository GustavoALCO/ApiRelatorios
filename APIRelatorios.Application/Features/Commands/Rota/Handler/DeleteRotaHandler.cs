using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class DeleteRotaHandler
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    public DeleteRotaHandler(IRotaCommands commands, IRotaQuery query)
    {
        _commands = commands;
        _query = query;
    }

    public async Task Handler(DeleteRotaCommand dltrota)
    {
        var rota = await _query.BuscarRotaID(dltrota.rotaId)
            ?? throw new Exception("Erro ao buscar rota");

        await _commands.DeleteRotaAsync(rota);
    }
}
