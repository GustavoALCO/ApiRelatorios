using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Images;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class UpdateDescricaoImageHandler
{
    private readonly IEvidenciaRotaQuery _query;

    private readonly IEvidenciaRotaCommands _commands;

    private readonly IValidateIds _validateids;

    public UpdateDescricaoImageHandler(IEvidenciaRotaQuery query, IEvidenciaRotaCommands commands, IValidateIds validateids)
    {
        _query = query;
        _commands = commands;
        _validateids = validateids;
    }

    public async Task Handler(UpdateEvidenciasCommands updateDescricao)
    {

        var image = await _query.GetEvidenciaId(updateDescricao.evidenciaId) ?? throw new Exception("Erro ao Encontrar Evidencia");

        image.Atualizar(
            updateDescricao.descricao,
            updateDescricao.tema,
            updateDescricao.alimentador,
            updateDescricao.endereco,
            updateDescricao.identificacao
            );

        await _commands.UpdateImageAsync(image);
    }
}
