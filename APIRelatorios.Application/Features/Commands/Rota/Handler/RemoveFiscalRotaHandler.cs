using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class RemoveFiscalRotaHandler 
    : ICommandHandler<RemoveFiscalRotaCommand>
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    private readonly IValidateIds _validateids;

    public RemoveFiscalRotaHandler(IRotaCommands commands, IRotaQuery query, IValidateIds validateids)
    {
        _commands = commands;
        _query = query;
        _validateids = validateids;
    }

    public async Task Handle(RemoveFiscalRotaCommand command, CancellationToken cancellationToken)
    {
        foreach (var fiscais in command.fiscaisId)
        {
            if (await _validateids.UserExisteAsync(fiscais) is false)
                throw new UserNotFoundException(fiscais);
        }

        var rota = await _query.BuscarRotaID(command.rotaId) 
            ?? throw new RotaNotFoundException();


        foreach (var userId in command.fiscaisId)
        {
            await _commands.RemoverFiscalRota(userId ,command.rotaId);
        }

    }
}
