using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;

public class BuscarTodasAsEvidenciasRotaHandler
{
    private readonly IEvidenciaRotaQuery _evidenciaRota;

    private readonly IUserQuery _userQuery;

    private readonly IImagesQuery _imagesQuery;

    public BuscarTodasAsEvidenciasRotaHandler(IEvidenciaRotaQuery evidenciaRota, IUserQuery userQuery, IImagesQuery imagesQuery)
    {
        _evidenciaRota = evidenciaRota;
        _userQuery = userQuery;
        _imagesQuery = imagesQuery;
    }

    public async Task<ICollection<EvidenciaDTO>> Handler(BuscarTodasEvidenciasRotaCommands commands)
    {
        var evidencia = await _evidenciaRota.GetEvidenciasPagination(commands.IdRota, commands.Page, commands.PageSize);

        if(evidencia.Count == 0)
        {
            throw new Exception("Não há evidencias para ser mostrada");
        }

        // Seleciona os ids dos fiscais
        var fiscalIds = evidencia
                                .Select(x => x.FiscalId)
                                .Distinct()
                                .ToList();

        var users = await _userQuery.BuscarListaFiscalIds(fiscalIds);

        List<EvidenciaDTO> evidenciaDTOs = new List<EvidenciaDTO>();


        foreach(var evidencias in evidencia)
        {
            var fiscal = users.First(x => x.UserId == evidencias.FiscalId);

            var images = await _imagesQuery.GetImageEvidencia(evidencias.EvidenciaRotaId) ?? throw new Exception("Não há imagens declaradas nesta evidencia");

            List<string> imageOriginal = new();
            List<string> imageMedium = new();
            List<string> imageLow = new();

            foreach (var image in images)
            {
                imageOriginal.Add(image.OriginalUrl);
                imageMedium.Add(image.MediumUrl);
                imageLow.Add(image.LowUrl);
            }


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
                MediumImageUrl = imageMedium,
                LowImageUrl = imageLow,
                Identificacao = evidencias.Identificacão,
                NomeFiscal = $"{fiscal.Name} {fiscal.LastName}",
                TemaFiscalizacao = ((int)evidencias.TemaFiscalizacao),
                Latitude = evidencias.Latitude,
                Longitude = evidencias.Longitude,
                Emergencial = evidencias.Emergencial
            };

            evidenciaDTOs.Add(evidenciaDTO);
        }

        return evidenciaDTOs;
    }
}
