using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;

public class BuscarEvidenciaPorIdHandler
    : IQueryHandler<BuscarEvidenciaPorIDQuery, EvidenciaDTO>
{
    private readonly IEvidenciaRotaQuery _evidenciaRota;

    private readonly IUserQuery _userquery;

    private readonly IImagesQuery _imagesquery;

    private readonly ILogger<BuscarEvidenciaPorIdHandler> _logger;

    public BuscarEvidenciaPorIdHandler(IEvidenciaRotaQuery evidenciaRota, IUserQuery userquery, IImagesQuery imagesquery, ILogger<BuscarEvidenciaPorIdHandler> logger)
    {
        _evidenciaRota = evidenciaRota;
        _userquery = userquery;
        _imagesquery = imagesquery;
        _logger = logger;
    }

    public async Task<EvidenciaDTO> Handle(BuscarEvidenciaPorIDQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando processo de busca da evidência com ID {EvidenciaId}", query.IdEvidencia);

        var evidencias = await _evidenciaRota.GetEvidenciaId(query.IdEvidencia) 
            ?? throw new EvidenciaNotFoundException(query.IdEvidencia);
        _logger.LogInformation("Evidência encontrada: {EvidenciaId}", evidencias.EvidenciaRotaId);

        _logger.LogInformation("Buscando informações do fiscal com ID {FiscalId}", evidencias.FiscalId);
        var fiscal = await _userquery.BuscarFiscalId(evidencias.FiscalId) 
            ?? throw new UserNotFoundException(evidencias.FiscalId);
        _logger.LogInformation("Informações do fiscal encontradas: {FiscalName} {FiscalLastName}", fiscal.Name, fiscal.LastName);

        List<string> imageOriginal = new();
        List<string> imageLow = new();

        _logger.LogInformation("Mapeando URLs das imagens para a evidência com ID {EvidenciaId}", evidencias.EvidenciaRotaId);
        foreach (var image in evidencias.Images)
        {
            _logger.LogInformation("Adicionando URLs da imagem: Original - {OriginalUrl}, Low - {LowUrl}",
                image.OriginalUrl,
                image.LowUrl);

            imageOriginal.Add(image.OriginalUrl);
            imageLow.Add(image.LowUrl);
        }

        _logger.LogInformation("URLs das imagens mapeadas com sucesso para a evidência com ID {EvidenciaId}",
            evidencias.EvidenciaRotaId);

        _logger.LogInformation("Criando DTO para a evidência com ID {EvidenciaId}", evidencias.EvidenciaRotaId);
        EvidenciaDTO evidenciaDTO = new EvidenciaDTO
        {
            EvidenciaRotaId = evidencias.EvidenciaRotaId,
            RotaId = evidencias.RotaId,
            Alimentador = evidencias.Alimentador,
            Descricao = evidencias.Descricao,
            Endereco = evidencias.Endereco,
            Horario = evidencias.Horario,
            ImageURL = imageOriginal,
            LowImageUrl = imageLow,
            Identificacao = evidencias.Identificacão,
            NomeFiscal = $"{fiscal.Name} {fiscal.LastName}",
            subTemaFiscalizacao = evidencias.CheckList.SubTemaAlimentadores.ToList(),
            temaFiscalizacao = evidencias.CheckList.TemaCheck,
            Latitude = evidencias.Latitude,
            Longitude = evidencias.Longitude,
        };
        
        _logger.LogInformation("DTO criado com sucesso para a evidência com ID {EvidenciaId}", evidencias.EvidenciaRotaId);
        return evidenciaDTO;
    }
}
