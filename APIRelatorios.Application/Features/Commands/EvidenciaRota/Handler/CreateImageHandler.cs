using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using ChatApplication.Application.Interfaces;
using System.Text.RegularExpressions;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class CreateImageHandler
{

    private readonly IEvidenciaRotaCommands _commands;

    private readonly ISavedImages _uploadImage;

    private readonly IRotaQuery _rotaQuery;

    private readonly IUserQuery _query;


    public CreateImageHandler(IEvidenciaRotaCommands commands, ISavedImages uploadImage, IUserQuery query, IRotaQuery rotaQuery)
    {
        _commands = commands;
        _uploadImage = uploadImage;
        _query = query;
        _rotaQuery = rotaQuery;
    }

    public async Task Handler(CreateEvidenciaCommand createImage)
    {

        var rota = await _rotaQuery.BuscarRotaID(createImage.rotaID) ?? throw new Exception("Erro ao Encontrar Rota");

        var fiscais = await _query.BuscarFiscalId(createImage.fiscalId) ?? throw new Exception("Erro ao encontrar filcal");
        
        if(string.IsNullOrEmpty(createImage.Alimentador))
            createImage.Alimentador = rota.Alimentador;
        

        var urlImages = await _uploadImage.UploadListBase64ImagesAsync(alimentador: createImage.Alimentador,
                                                                       fiscal: $"{fiscais.Name}_{fiscais.LastName}",
                                                                       horario: createImage.Horario.ToString("yyyy_MM_dd__HH_mm"),
                                                                       base64Image: createImage.Base64,
                                                                       container: "imagens",
                                                                       evidenciaId: createImage.evidenciaId);

        if (urlImages.Count() == 0)
        {
            Console.WriteLine("Erro ao fazer upload da imagem");
            throw new Exception("Erro ao fazer upload da imagem");
        }

        Dommain.Entities.EvidenciaRota image = new(
            evidenciaRotaId: createImage.evidenciaId,
            rotaID: createImage.rotaID,
            fiscalId: createImage.fiscalId
            ,tema: createImage.TemaFiscalizacao
            ,alimentador: createImage.Alimentador 
            ,identificacao: createImage.Identificacao
            ,descricao: createImage.Descricao
            ,imagem: urlImages,
            endereco: createImage.Endereco,
            lat: createImage.Latitude,
            lon: createImage.Longitude,
            horario: createImage.Horario.AddHours(-3)
            );
        
        await _commands.SaveImage(image);
    }
}
