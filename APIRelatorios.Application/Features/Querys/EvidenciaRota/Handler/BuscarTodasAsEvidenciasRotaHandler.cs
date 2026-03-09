using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;

public class BuscarTodasAsEvidenciasRotaHandler
{
    private readonly IEvidenciaRotaQuery _evidenciaRota;

    private readonly IUserQuery _userQuery;

    public BuscarTodasAsEvidenciasRotaHandler(IEvidenciaRotaQuery evidenciaRota, IUserQuery userQuery)
    {
        _evidenciaRota = evidenciaRota;
        _userQuery = userQuery;
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

            EvidenciaDTO evidenciaDTO = new EvidenciaDTO
            {
                EvidenciaRotaId = evidencias.EvidenciaRotaId,
                RotaId = evidencias.RotaId,
                Alimentador = evidencias.Alimentador,
                Descricao = evidencias.Descricao,
                Endereco = evidencias.Endereco,
                Cep  =  evidencias.Cep, 
                Horario = evidencias.Horario.ToString("dd/MM/yyyy HH:mm"),
                ImageURL = evidencias.ImageURL,
                Identificacao = evidencias.Identificacão,
                NomeFiscal = $"{fiscal.Name} {fiscal.LastName}",
                TemaFiscalizacao = ((int)evidencias.TemaFiscalizacao),
            };

            evidenciaDTOs.Add(evidenciaDTO);
        }

        return evidenciaDTOs;
    }
}
