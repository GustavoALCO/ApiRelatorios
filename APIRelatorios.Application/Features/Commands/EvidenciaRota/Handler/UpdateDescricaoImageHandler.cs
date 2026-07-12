using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Domain.Enuns;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;
using APIRelatorios.Dommain.Interfaces.Images;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class UpdateDescricaoImageHandler
    : ICommandHandler<UpdateEvidenciasCommands>
{
    private readonly IEvidenciaRotaQuery _query;

    private readonly IEvidenciaRotaCommands _commands;

    private readonly ILogger<UpdateDescricaoImageHandler> _logger;

    public UpdateDescricaoImageHandler(
        IEvidenciaRotaQuery query,
        IEvidenciaRotaCommands commands,
        ILogger<UpdateDescricaoImageHandler> logger
        )
    {
        _query = query;
        _commands = commands;
        _logger = logger;
    }

    public async Task Handle(UpdateEvidenciasCommands updateDescricao, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando validacao de Id da rota");

        var image =
            await _query.GetEvidenciaId(
                updateDescricao.evidenciaId)
            ?? throw new RotaNotFoundException(updateDescricao.evidenciaId);

        _logger.LogInformation("Evidencia Encontrada, iniciando processo de atualização");

        // Atualiza Checklist
        image.CheckList.TemaCheck =
            (TemaCheck)updateDescricao.temaFiscalizacao;

        _logger.LogInformation("Tema de Fiscalização atualizado, iniciando atualização de SubTema");

        image.CheckList.SubTemaAlimentadores =
            updateDescricao.subTemaFiscalizacao
                .Distinct()
                .Select(x =>
                    (SubTemaAlimentadores)x)
                .ToList();

        _logger.LogInformation("SubTema atualizado, iniciando processo de atualização da entidade");

        var checkList = new CheckList(
        image.RotaId,
        (TemaCheck)updateDescricao.temaFiscalizacao,
        updateDescricao.subTemaFiscalizacao
            .Distinct()
            .Select(x =>
                (SubTemaAlimentadores)x)
                .ToList()
        );

        var nivelRisco = updateDescricao.nivelRisco is not null
            ? (NivelRisco)updateDescricao.nivelRisco
            : image.NivelRisco;

        // Atualiza entidade
        image.Atualizar(
            updateDescricao.descricao,
            checkList,
            updateDescricao.alimentador,
            updateDescricao.endereco,
            updateDescricao.identificacao,
            nivelRisco
        );

        await _commands.UpdateImageAsync(image);
    }
}