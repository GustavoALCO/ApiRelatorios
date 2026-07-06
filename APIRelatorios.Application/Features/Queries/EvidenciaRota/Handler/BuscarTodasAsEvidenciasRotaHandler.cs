using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;

public class BuscarTodasAsEvidenciasRotaHandler
    : IQueryHandler<BuscarTodasEvidenciasRotaQuery, ICollection<EvidenciaDTO>>
{
    private readonly IEvidenciaRotaQuery _evidenciaRota;

    private readonly IUserQuery _userQuery;

    private readonly IImagesQuery _imagesQuery;

    private readonly ILogger<BuscarTodasAsEvidenciasRotaHandler> _logger;

    public BuscarTodasAsEvidenciasRotaHandler(IEvidenciaRotaQuery evidenciaRota, IUserQuery userQuery, IImagesQuery imagesQuery, ILogger<BuscarTodasAsEvidenciasRotaHandler> logger)
    {
        _evidenciaRota = evidenciaRota;
        _userQuery = userQuery;
        _imagesQuery = imagesQuery;
        _logger = logger;
    }

    public async Task<ICollection<EvidenciaDTO>> Handle(BuscarTodasEvidenciasRotaQuery commands, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando busca de evidências para a rota {RotaId} na página {Page} com tamanho de página {PageSize}",
            commands.IdRota, 
            commands.Page, 
            commands.PageSize
            );

        var evidencia = await _evidenciaRota.GetEvidenciasPagination(commands.IdRota, commands.Page, commands.PageSize)
            ?? throw new EvidenciaNotFoundException(commands.IdRota);

        _logger.LogInformation("Encontradas {Count} evidências para a rota {RotaId}", 
            evidencia.Count, 
            commands.IdRota
            );

        _logger.LogInformation("Buscando informações dos fiscais relacionados às evidências");
        // Seleciona os ids dos fiscais
        var fiscalIds = evidencia
                                .Select(x => x.FiscalId)
                                .Distinct()
                                .ToList();

        _logger.LogInformation("Encontrados {Count} fiscais distintos relacionados às evidências", fiscalIds.Count);

        _logger.LogInformation("Buscando informações dos fiscais no banco de dados");

        var users = await _userQuery.BuscarListaFiscalIds(fiscalIds)
            ?? throw new ListUsersNotFoundException();

        _logger.LogInformation("Encontradas {Count} informações de fiscais no banco de dados", users.Count);

        List<EvidenciaDTO> evidenciaDTOs = new List<EvidenciaDTO>();

        foreach(var evidencias in evidencia)
        {
            _logger.LogInformation("Processando evidência {EvidenciaRotaId} para a rota {RotaId}",
                evidencias.EvidenciaRotaId, 
                evidencias.RotaId
                );

            var fiscal = users.First(x => x.UserId == evidencias.FiscalId)
                            ?? throw new UserNotFoundException(evidencias.FiscalId);
            
            var images = await _imagesQuery.GetImageEvidencia(evidencias.EvidenciaRotaId) 
                ?? throw new Exception("Não há imagens declaradas nesta evidencia");

            _logger.LogInformation("Encontradas {Count} imagens para a evidência {EvidenciaRotaId}", images.Count, evidencias.EvidenciaRotaId);

            List<string> imageOriginal = new();
            List<string> imageLow = new();

            foreach (var image in images)
            {
                imageOriginal.Add(image.OriginalUrl);
                imageLow.Add(image.LowUrl);
            }
            _logger.LogInformation("Criando DTO para a evidência {EvidenciaRotaId}", evidencias.EvidenciaRotaId);

            EvidenciaDTO evidenciaDTO = new EvidenciaDTO
            {
                EvidenciaRotaId = evidencias.EvidenciaRotaId,
                RotaId = evidencias.RotaId,
                Alimentador = evidencias.Alimentador,
                Descricao = evidencias.Descricao,
                Endereco = evidencias.Endereco,
                Cidade = evidencias.Cidade,
                Horario = evidencias.Horario,
                ImageURL = imageOriginal,
                LowImageUrl = imageLow,
                Identificacao = evidencias.Identificacão,
                NomeFiscal = $"{fiscal.Name} {fiscal.LastName}",
                subTemaFiscalizacao = evidencias.CheckList.SubTemaAlimentadores,
                temaFiscalizacao = evidencias.CheckList.TemaCheck,
                Latitude = evidencias.Latitude,
                Longitude = evidencias.Longitude,
                NivelRisco = evidencias.NivelRisco
            };
            _logger.LogInformation("DTO criado para a evidência {EvidenciaRotaId}, adicionando à lista de resultados",
                evidenciaDTO.EvidenciaRotaId);

            evidenciaDTOs.Add(evidenciaDTO);
        }
        _logger.LogInformation("Processamento concluído para todas as evidências da rota {RotaId}. Total de DTOs criados: {Count}",
            commands.IdRota,
            evidenciaDTOs.Count);
        return evidenciaDTOs;
    }
}
