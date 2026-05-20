using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;

public class BuscarEvidenciaPorIdHandler
{
    private readonly IEvidenciaRotaQuery _evidenciaRota;

    private readonly IUserQuery _userquery;

    private readonly IImagesQuery _imagesquery;

    public BuscarEvidenciaPorIdHandler(IEvidenciaRotaQuery evidenciaRota, IUserQuery userquery, IImagesQuery imagesquery)
    {
        _evidenciaRota = evidenciaRota;
        _userquery = userquery;
        _imagesquery = imagesquery;
    }

    public async Task<EvidenciaDTO> Handler(Guid commands)
    {
        var evidencias = await _evidenciaRota.GetEvidenciaId(commands) ?? throw new Exception("Não existe Evidencia com esse ID");
 
        

        var fiscal = await _userquery.BuscarFiscalId(evidencias.FiscalId) ?? throw new Exception("Erro ao Encontrar Fiscal com o Id armazenado");


        var images = await _imagesquery.GetImageEvidencia(evidencias.EvidenciaRotaId) ?? throw new Exception("Não há imagens declaradas nesta evidencia");

        List<string> imageOriginal = new();
        List<string> imageLow = new();

        foreach (var image in images)
        {
            imageOriginal.Add(image.OriginalUrl);
            imageLow.Add(image.LowUrl);
        }

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
        

        return evidenciaDTO;
    }
}
