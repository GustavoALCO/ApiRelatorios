using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public sealed class AddFiscalRotaHandler
        : ICommandHandler<AddFiscalRotaCommand>
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    private readonly IValidateIds _validateids;

    private readonly ILogger<AddFiscalRotaHandler> _logger;

    public AddFiscalRotaHandler(IRotaQuery query, IRotaCommands commands, IValidateIds validateids, ILogger<AddFiscalRotaHandler> logger)
    {
        _query = query;
        _commands = commands;
        _validateids = validateids;
        _logger = logger;
    }
    public async Task Handle(AddFiscalRotaCommand add, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fazendo verificação a lista de fiscais é valida");
        foreach (var fiscais in add.FiscaisId)
        {
            if (await _validateids.UserExisteAsync(fiscais) is false)
                throw new ListUsersNotFoundException();
        }
        _logger.LogInformation("Lista é Valida");

        var rota = await _query.BuscarRotaID(add.rotaId) ?? 
            throw new RotaNotFoundException();

        _logger.LogInformation("Iniciando loop de para adicionar fiscal");
        foreach (var fiscalAdd in add.FiscaisId)
        {
            UsuarioRota user = new()
            { RotaId = add.rotaId,
            UserId = fiscalAdd};

            await _commands.AdicionarFiscalRota(user);
        }

        _logger.LogInformation("Finalizado processo de adicionar fiscal");
    }
}
