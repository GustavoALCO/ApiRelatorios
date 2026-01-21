using APIRelatorios.Dommain.Interfaces.Images;
using ChatApplication.Application.Interfaces;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class CreateImage
{

    private readonly IImageCommands _commands;

    private readonly ISavedImages _uploadImage;

    public CreateImage(IImageCommands commands, ISavedImages uploadImage)
    {
        _commands = commands;
        _uploadImage = uploadImage;
    }

    public async Task Handler(CreateImageCommand createImage)
    {
        var urlImage = await _uploadImage.UploadBase64ImagesAsync(createImage.Base64, "teste")
            ?? throw new Exception("Erro ao Enviar Mensagem a nuvem");

        Dommain.Entities.Imagem image = new()
        {
            Horario = createImage.Horario,
            Cep = createImage.Cep,
            Descricao = createImage.Descricao,
            Endereco = createImage.Endereco,
            ImageURL = urlImage,
            Latitude = createImage.Latitude,
            Longitude = createImage.Longitude,
            RotaID = createImage.rotaID
        };

        await _commands.SaveImage(image);
    }
}
