using APIRelatorios.Dommain.Interfaces.Rota;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class UpdateNomeRotaHandler
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    public UpdateNomeRotaHandler(IRotaCommands commands, IRotaQuery query)
    {
        _commands = commands;
        _query = query;
    }

    public async Task Handler(UpdateNomeRotaCommand updNome)
    {
        var rota = await _query.BuscarRotaID(updNome.rotaId) ??
            throw new Exception("Erro ao Encontrar Rota no Banco de dados"); ;

        rota.NomeRota = updNome.nomeRota;

        await _commands.UpdateRotaAsync(rota);
    }
}
