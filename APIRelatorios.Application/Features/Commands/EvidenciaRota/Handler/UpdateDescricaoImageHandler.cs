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

    public async Task Handler(UpdateDescricaoImageCommands updateDescricao)
    {

        if (await _validateids.EvidenciaExisteAsync(updateDescricao.idDescricao) is false)
            throw new Exception("Id invalido");

        var image = await _query.GetImageId(updateDescricao.idDescricao) ?? throw new Exception("Erro ao Encontrar imagem");

        image.AlterarInformacoes(
            image.Descricao,
            updateDescricao.descricao,
            image.Identificação);

        await _commands.UpdateImageAsync(image);
    }
}
