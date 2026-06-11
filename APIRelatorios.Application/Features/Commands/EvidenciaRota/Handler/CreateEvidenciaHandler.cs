using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Domain.Interfaces.Services;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;
using APIRelatorios.Dommain.Helpers;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using ChatApplication.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace APIRelatorios.Application.Features.Commands.Images.Handler;

public class CreateEvidenciaHandler
    : ICommandHandler<CreateEvidenciaCommand>
{

    private readonly IEvidenciaRotaCommands _commands;

    private readonly ISavedImages _uploadImage;

    private readonly IRotaQuery _rotaQuery;

    private readonly IUserQuery _query;

    private readonly IAzureMapsEnderecoService _mapsEnderecoService;

    private readonly ILogger<CreateEvidenciaHandler> _logger;

    public CreateEvidenciaHandler(IEvidenciaRotaCommands commands, ISavedImages uploadImage, IUserQuery query, IRotaQuery rotaQuery, IAzureMapsEnderecoService mapsEnderecoService, ILogger<CreateEvidenciaHandler> logger)
    {
        _commands = commands;
        _uploadImage = uploadImage;
        _query = query;
        _rotaQuery = rotaQuery;
        _mapsEnderecoService = mapsEnderecoService;
        _logger = logger;
    }

    public async Task Handle(CreateEvidenciaCommand createImage, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Iniciando processo de criação de evidencia para a rota");

        var rota = await _rotaQuery.BuscarRotaID(createImage.rotaID) ?? throw new Exception("Erro ao Encontrar Rota");

        var fiscais = await _query.BuscarFiscalId(createImage.fiscalId) ?? throw new Exception("Erro ao encontrar filcal");

        if (rota.DataFinal > createImage.Horario)
            throw new Exception("Rota Já finalizada.");

        _logger.LogInformation("Rota Encontrada, iniciando processo de criação da evidencia");

        if(string.IsNullOrEmpty(createImage.Alimentador))
            createImage.Alimentador = rota.Alimentador;

        _logger.LogInformation("Alimentador atribuido, iniciando processo de validação de endereço e cidade");

        if (string.IsNullOrEmpty(createImage.Cidade) || string.IsNullOrEmpty(createImage.Endereco))
        {
            var azurereturn = await _mapsEnderecoService.BuscarNomeRua(createImage.Latitude, createImage.Longitude) 
                                                        ?? throw new Exception("Erro ao buscar nome da rua e cidade");

            if (string.IsNullOrEmpty(createImage.Cidade))
            {
                _logger.LogInformation("Atribuindo valor de cidade");
                createImage.Cidade = azurereturn.Item2;
            }
                

            if (string.IsNullOrEmpty(createImage.Endereco))
            {
                _logger.LogInformation("Atribuindo valor do endereço");
                createImage.Endereco = azurereturn.Item1;
            }
                
        }
        
        _logger.LogInformation("Endereço e Cidade validados, iniciando processo de upload da imagem");

        var urlImages = await _uploadImage.UploadListBase64ImagesAsync(alimentador: createImage.Alimentador,
                                                                       fiscal: $"{fiscais.Name}_{fiscais.LastName}",
                                                                       horario: createImage.Horario.AddHours(-3).ToString("yyyy/MM/dd HH:mm"),
                                                                       base64Images: createImage.Base64,
                                                                       container: "testes",
                                                                       rua: createImage.Endereco,
                                                                       evidenciaId: createImage.evidenciaId,
                                                                       lat: createImage.Latitude,
                                                                       log: createImage.Longitude);

        if (urlImages.Count() == 0)
        {
            _logger.LogInformation("Erro ao fazer upload da imagem");
            throw new Exception("Erro ao fazer upload da imagem");
        }

        _logger.LogInformation("Imagem salva com sucesso, iniciando processo de criação da entidade de evidencia");

        // Cria Classe de checklist para salvar o tema e subtema da fiscalização
        CheckList checkList = new CheckList(
            createImage.evidenciaId,
            (TemaCheck)createImage.temaFiscalizacao,
            createImage.subTemaFiscalizacao
                .Select(x => (SubTemaAlimentadores)x)
                .ToList()
        );
        
        Dommain.Entities.EvidenciaRota image = new(
            evidenciaRotaId: createImage.evidenciaId,
            rotaID: createImage.rotaID,
            fiscalId: createImage.fiscalId
            ,checkList: checkList
            ,alimentador: createImage.Alimentador 
            ,identificacao: createImage.Identificacao
            ,descricao: createImage.Descricao
            ,imagem: urlImages,
            endereco: createImage.Endereco,
            cidade: createImage.Cidade,
            lat: createImage.Latitude,
            lon: createImage.Longitude,
            horario: createImage.Horario.AddHours(-3),
            emergencial: createImage.Emergencial
            );
        
        await _commands.SaveImage(image);

        _logger.LogInformation("Evidencia criada com sucesso");
    }
}
