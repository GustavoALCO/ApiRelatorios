using APIRelatorios.Dommain.Interfaces.Images;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class UpdateDescricaoImageHandler
{
    private readonly IImageQuery _query;

    private readonly IImageCommands _commands;
    public UpdateDescricaoImageHandler(IImageQuery query, IImageCommands commands)
    {
        _query = query;
        _commands = commands;
    }

    public async Task Handler(UpdateDescricaoImageCommands updateDescricao)
    {
        var image = await _query.GetImageId(updateDescricao.idMensage) ?? throw new Exception("Erro ao Encontrar imagem");

        image.Descricao = updateDescricao.descricao;

        await _commands.UpdateImageAsync(image);
    }
}
