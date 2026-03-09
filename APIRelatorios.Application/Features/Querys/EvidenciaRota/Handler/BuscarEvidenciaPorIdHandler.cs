using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.User;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;

public class BuscarEvidenciaPorIdHandler
{
    private readonly IEvidenciaRotaQuery _evidenciaRota;

    private readonly IUserQuery _userquery;

    public BuscarEvidenciaPorIdHandler(IEvidenciaRotaQuery evidenciaRota, IUserQuery userquery)
    {
        _evidenciaRota = evidenciaRota;
        _userquery = userquery;
    }

    public async Task<EvidenciaDTO> Handler(int commands)
    {
        var evidencias = await _evidenciaRota.GetImageId(commands) ?? throw new Exception("Não existe Evidencia com esse ID");
 
        

        var fiscal = await _userquery.BuscarFiscalId(evidencias.FiscalId) ?? throw new Exception("Erro ao Encontrar Fiscal com o Id armazenado");

        EvidenciaDTO evidenciaDTO = new EvidenciaDTO
        {
            EvidenciaRotaId = evidencias.EvidenciaRotaId,
            RotaId = evidencias.RotaId,
            Alimentador = evidencias.Alimentador,
            Descricao = evidencias.Descricao,
            Endereco = evidencias.Endereco,
            Cep = evidencias.Cep,
            Horario = evidencias.Horario.ToString(),
            ImageURL = evidencias.ImageURL,
            Identificacao = evidencias.Identificacão,
            NomeFiscal = $"{fiscal.Name} {fiscal.LastName}",
            TemaFiscalizacao = ((int)evidencias.TemaFiscalizacao),
        };
        

        return evidenciaDTO;
    }
}
