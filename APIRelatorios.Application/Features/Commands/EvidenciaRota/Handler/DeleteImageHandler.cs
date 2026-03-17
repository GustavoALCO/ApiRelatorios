using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Images;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class DeleteImageHandler
{
    private readonly IEvidenciaRotaQuery _query;

    private readonly IEvidenciaRotaCommands _commands;

    private readonly IValidateIds _validateids;

    public DeleteImageHandler(IEvidenciaRotaQuery query, IEvidenciaRotaCommands commands, IValidateIds validateids)
    {
        _query = query;
        _commands = commands;
        _validateids = validateids;
    }

    public async Task Handler(int updateDescricao)
    {
        if (await _validateids.EvidenciaExisteAsync(updateDescricao) is false)
            throw new Exception("Id invalido");

        var image = await _query.GetEvidenciaId(updateDescricao) ?? throw new Exception("Erro ao Encontrar imagem");

        await _commands.DeleteImage(image);
    }
}
