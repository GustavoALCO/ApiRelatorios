using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using ChatApplication.Application.Interfaces;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class CreateImageHandler
{

    private readonly IEvidenciaRotaCommands _commands;

    private readonly ISavedImages _uploadImage;

    private readonly IRotaQuery _rotaQuery;

    private readonly IUserQuery _query;
    private readonly IValidateBase64 _validateBase64;

    public CreateImageHandler(IEvidenciaRotaCommands commands, ISavedImages uploadImage, IValidateBase64 validateBase64, IUserQuery query, IRotaQuery rotaQuery)
    {
        _commands = commands;
        _uploadImage = uploadImage;
        _validateBase64 = validateBase64;
        _query = query;
        _rotaQuery = rotaQuery;
    }

    public async Task Handler(CreateImageCommand createImage)
    {

        var rota = await _rotaQuery.BuscarRotaID(createImage.rotaID) ?? throw new Exception("Erro ao Encontrar Rota");

        var fiscais = await _query.BuscarFiscalId(createImage.fiscalId) ?? throw new Exception("Erro ao encontrar filcal");
        
        if(string.IsNullOrEmpty(createImage.Alimentador))
            createImage.Alimentador = rota.Alimentador;


        var urlImage = await _uploadImage.UploadBase64ImagesAsync(createImage.Alimentador,
                                                                  fiscal: $"{fiscais.Name}_{fiscais.LastName}",
                                                                  createImage.Horario.ToString("yyyy_MM_dd__HH_mm"),
                                                                  createImage.Base64,
                                                                  "imagens");
        
            if(string.IsNullOrEmpty(urlImage))
        {
            Console.WriteLine("Erro ao fazer upload da imagem");
            throw new Exception("Erro ao fazer upload da imagem");
        }

        Dommain.Entities.EvidenciaRota image = new(
            rotaID: createImage.rotaID,
            fiscalId: createImage.fiscalId
            ,tema: createImage.TemaFiscalizacao
            ,alimentador: createImage.Alimentador 
            ,identificacao: createImage.Identificacao
            ,descricao: createImage.Descricao
            ,imagem: urlImage,
            endereco: createImage.Endereco,
            cep: createImage.Cep,
            lat: createImage.Latitude,
            lon: createImage.Longitude,
            horario: createImage.Horario.AddHours(-3)
            );
        
        await _commands.SaveImage(image);
    }
}
