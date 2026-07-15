using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Rota;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class UpdateNomeRotaHandler
    : ICommandHandler<UpdateNomeRotaCommand>
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

    public async Task Handle(UpdateNomeRotaCommand updNome, CancellationToken cancellationToken)
    {

        var rota = await _query.BuscarRotaID(updNome.rotaId) ??
            throw new RotaNotFoundException(); ;

        rota.AlterarNomeRota(updNome.nomeRota);

        await _commands.UpdateRotaAsync(rota);
    }
}
