using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Domain.Interfaces.Amostra;
using APIRelatorios.Dommain.Interfaces.User;
using ChatApplication.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Amostra.Handler;

public class UpdateAmostraHandler : ICommandHandler<UpdateAmostraCommand>
{
    private readonly ILogger<UpdateAmostraHandler> _logger;

    private readonly IAmostraCommands _commands;

    private readonly IAmostraQuery _query;

    private readonly IUserQuery _userQuery;

    private readonly ISavedImages _imageService;

    public UpdateAmostraHandler(IAmostraQuery query, IAmostraCommands commands, ILogger<UpdateAmostraHandler> logger, ISavedImages imageService, IUserQuery userQuery)
    {
        _query = query;
        _commands = commands;
        _logger = logger;
        _imageService = imageService;
        _userQuery = userQuery;
    }

    public async Task Handle(UpdateAmostraCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Iniciando o Processo de Atualizar: {command.Id}");

        var amostra = await _query.GetAmostraId(command.Id) ?? throw new AmostraNotFoundException();
        
        _logger.LogInformation($"Amostra encontrada: {amostra.Id}");

        var user = await _userQuery.BuscarFiscalId(command.IdFiscal) ?? throw new UserNotFoundException(command.IdFiscal);

        _logger.LogInformation("Enviando Imagens para o serviço de armazenamento...");

        var urlImages = await _imageService.UploadListBase64ImagesAsync(
                                           alimentador: amostra.SeqISA,
                                           fiscal: $"{user.Name}-{user.LastName}",
                                           "",
                                           command.fotos,
                                           "images",
                                           "-",
                                           amostra.RotaId,
                                           0,
                                           0
                                );

        if ( urlImages.Count() == 0 ) 
            throw new AmostraNotFoundException();

        _logger.LogInformation($"Atualizando a Amostra: {amostra.Id}");

        amostra.AtualizarEquipamentosAmostra(
            command.tuc1,
            command.tuc2,
            command.tuc3,
            command.tuc4,
            command.tuc5,
            command.tuc6,
            command.posicaoOperativa,
            command.equipamento,
            command.numSerie,
            command.dataFabricacao,
            command.observacao,
            urlImages.Select(x => x.OriginalUrl).ToList(),
            command.latitude,
            command.longitude
        );

        _logger.LogInformation($"Finalizando a Atualização da Amostra: {amostra.Id}");

        _logger.LogInformation($"Salvando a Amostra: {amostra.Id}");

        await _commands.UpdateAmostra(amostra);

        _logger.LogInformation($"Finalizando o Processo de Atualizar: {command.Id}");
    }
}
