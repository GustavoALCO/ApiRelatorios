using APIRelatorios.Dommain.Interfaces.Images;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class UpdateDescricaoImageHandler
{
    private readonly IEvidenciaRotaQuery _query;

    private readonly IEvidenciaRotaCommands _commands;
    public UpdateDescricaoImageHandler(IEvidenciaRotaQuery query, IEvidenciaRotaCommands commands)
    {
        _query = query;
        _commands = commands;
    }

    public async Task Handler(UpdateDescricaoImageCommands updateDescricao)
    {
        var image = await _query.GetImageId(updateDescricao.idMensage) ?? throw new Exception("Erro ao Encontrar imagem");

        image.AlterarDescricao(updateDescricao.descricao);

        await _commands.UpdateImageAsync(image);
    }
}
