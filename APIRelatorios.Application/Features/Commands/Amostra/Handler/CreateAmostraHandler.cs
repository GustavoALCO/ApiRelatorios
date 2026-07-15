using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Domain.Interfaces.Amostra;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Amostra.Handler;

public class CreateAmostraHandler : ICommandHandler<CreateAmostraCommand>
{
    private readonly ILogger<CreateAmostraHandler> _logger;

    private readonly IAmostraCommands _commands;

    public CreateAmostraHandler(IAmostraCommands commands, ILogger<CreateAmostraHandler> logger)
    {
        _commands = commands;
        _logger = logger;
    }

    public async Task Handle(CreateAmostraCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Iniciando o Processo de Adicionar: {command.SeqISA}");

        Domain.Entities.Amostra amostra = new();

        amostra.createEquipamentoAmostra(
            command.RotaId,
            command.SeqISA,
            command.SeqBaseFisica,
            command.VlrBase,
            command.DescricaoTUC,
            command.DescricaoTec,
            command.ODIEngenharia,
            command.Instalacao,
            command.Endereco,
            command.Municipio,
            command.Latitude,
            command.Longitude,
            command.TUC1,
            command.TUC2 ?? string.Empty,
            command.TUC3 ?? string.Empty,
            command.TUC4 ?? string.Empty,
            command.TUC5 ?? string.Empty,
            command.TUC6 ?? string.Empty,
            command.NumSerie ?? string.Empty,
            command.PosicaoOperativa ?? string.Empty,
            command.Equipamento ?? string.Empty
        );

        await _commands.SaveAmostra(amostra);
    }
}