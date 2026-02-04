using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Rota;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class UpdateNomeRotaHandler
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    private readonly IValidateIds _validateIds;

    public UpdateNomeRotaHandler(IRotaCommands commands, IRotaQuery query, IValidateIds validateIds)
    {
        _commands = commands;
        _query = query;
        _validateIds = validateIds;
    }

    public async Task Handler(UpdateNomeRotaCommand updNome)
    {
        if (await _validateIds.RotaExisteAsync(updNome.rotaId) is false)
            throw new Exception("Id invalido");

        var rota = await _query.BuscarRotaID(updNome.rotaId) ??
            throw new Exception("Erro ao Encontrar Rota no Banco de dados"); ;

        rota.AlterarNomeRota(updNome.nomeRota);

        await _commands.UpdateRotaAsync(rota);
    }
}
