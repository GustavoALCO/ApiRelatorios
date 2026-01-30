using APIRelatorios.Dommain.Interfaces.Images;
using ChatApplication.Application.Interfaces;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class CreateImageHandler
{

    private readonly IEvidenciaRotaCommands _commands;

    private readonly ISavedImages _uploadImage;

    public CreateImageHandler(IEvidenciaRotaCommands commands, ISavedImages uploadImage)
    {
        _commands = commands;
        _uploadImage = uploadImage;
    }

    public async Task Handler(CreateImageCommand createImage)
    {
        var urlImage = await _uploadImage.UploadBase64ImagesAsync(createImage.Base64, "imagens")
            ?? throw new Exception("Erro ao Enviar Imagem a nuvem");

        Dommain.Entities.EvidenciaRota image = new(createImage.rotaID
            ,createImage.TemaFiscalizacao
            ,createImage.Alimentador
            ,createImage.Descricao
            ,urlImage,
            createImage.Endereco,
            createImage.Cep,
            createImage.Latitude,
            createImage.Longitude);
        
        await _commands.SaveImage(image);
    }
}
