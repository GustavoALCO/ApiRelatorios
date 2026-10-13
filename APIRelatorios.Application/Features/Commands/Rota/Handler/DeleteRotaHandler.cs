using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Rota;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class DeleteRotaHandler
    : ICommandHandler<DeleteRotaCommand>
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    private readonly ILogger<DeleteRotaHandler> _logger;

    public DeleteRotaHandler(IRotaCommands commands, IRotaQuery query, ILogger<DeleteRotaHandler> logger)
    {
        _commands = commands;
        _query = query;
        _logger = logger;
    }

    public async Task Handle(DeleteRotaCommand command, CancellationToken cancellationToken)
    {    
        _logger.LogInformation($"Incio da Exclusão da Rota: {command.RotaId}");

        var rota = await _query.BuscarRotaID(command.RotaId)
            ?? throw new RotaNotFoundException();

        // Altera o estado da rota para excluída
        rota.ExcluirRota();

        await _commands.UpdateRotaAsync(rota);
    }
}
