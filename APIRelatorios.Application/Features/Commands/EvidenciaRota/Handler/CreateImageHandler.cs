using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Images;
using ChatApplication.Application.Interfaces;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class CreateImageHandler
{

    private readonly IEvidenciaRotaCommands _commands;

    private readonly ISavedImages _uploadImage;


    private readonly IValidateBase64 _validateBase64;

    public CreateImageHandler(IEvidenciaRotaCommands commands, ISavedImages uploadImage, IValidateBase64 validateBase64)
    {
        _commands = commands;
        _uploadImage = uploadImage;
        _validateBase64 = validateBase64;
    }

    public async Task Handler(CreateImageCommand createImage)
    {

        if (_validateBase64.IsValidBase64String(createImage.Base64) is false) 
            throw new Exception("Imagem não esta no formato correto");

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
