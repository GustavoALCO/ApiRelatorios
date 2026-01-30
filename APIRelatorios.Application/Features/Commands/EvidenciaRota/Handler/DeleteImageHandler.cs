using APIRelatorios.Dommain.Interfaces.Images;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class DeleteImageHandler
{
    private readonly IEvidenciaRotaQuery _query;

    private readonly IEvidenciaRotaCommands _commands;
    public DeleteImageHandler(IEvidenciaRotaQuery query, IEvidenciaRotaCommands commands)
    {
        _query = query;
        _commands = commands;
    }

    public async Task Handler(DeleteImageCommands updateDescricao)
    {
        var image = await _query.GetImageId(updateDescricao.idImage) ?? throw new Exception("Erro ao Encontrar imagem");

        await _commands.DeleteImage(image);
    }
}
