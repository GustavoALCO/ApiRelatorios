using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class DeleteRotaHandler
    : ICommandHandler<DeleteRotaCommand>
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    private readonly IValidateIds _validateIds;

    public DeleteRotaHandler(IRotaCommands commands, IRotaQuery query, IValidateIds validateIds)
    {
        _commands = commands;
        _query = query;
        _validateIds = validateIds;
    }

    public async Task Handle(DeleteRotaCommand command, CancellationToken cancellationToken)
    {    

        var rota = await _query.BuscarRotaID(command.RotaId)
            ?? throw new RotaNotFoundException();

        // Altera o estado da rota para excluída
        rota.ExcluirRota();

        await _commands.DeleteRotaAsync(rota);
    }
}
