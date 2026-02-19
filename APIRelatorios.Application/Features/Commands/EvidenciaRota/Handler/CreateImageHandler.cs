using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.User;
using ChatApplication.Application.Interfaces;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class CreateImageHandler
{

    private readonly IEvidenciaRotaCommands _commands;

    private readonly ISavedImages _uploadImage;

    private readonly IUserQuery _query;
    private readonly IValidateBase64 _validateBase64;

    public CreateImageHandler(IEvidenciaRotaCommands commands, ISavedImages uploadImage, IValidateBase64 validateBase64, IUserQuery query)
    {
        _commands = commands;
        _uploadImage = uploadImage;
        _validateBase64 = validateBase64;
        _query = query;
    }

    public async Task Handler(CreateImageCommand createImage)
    {
        
        var fiscais = await _query.BuscarListaFiscalId(createImage.fiscalId) ?? throw new Exception("Erro ao encontrar filcal");
        
        if (_validateBase64.IsValidBase64String(createImage.Base64) is false) 
            throw new Exception("Imagem não esta no formato correto");

        var urlImage = await _uploadImage.UploadBase64ImagesAsync(createImage.Base64, "imagens")
            ?? throw new Exception("Erro ao Enviar Imagem a nuvem");

        Dommain.Entities.EvidenciaRota image = new(
            createImage.rotaID,
            createImage.fiscalId
            ,createImage.TemaFiscalizacao
            ,createImage.Identificacao
            ,createImage.Alimentador
            ,createImage.Descricao
            ,urlImage,
            createImage.Endereco,
            createImage.Cep,
            createImage.Latitude,
            createImage.Longitude
            );
        
        await _commands.SaveImage(image);
    }
}
