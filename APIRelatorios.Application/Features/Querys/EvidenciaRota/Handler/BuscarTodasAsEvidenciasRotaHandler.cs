using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;

public class BuscarTodasAsEvidenciasRotaHandler
{
    private readonly IEvidenciaRotaQuery _evidenciaRota;

    public BuscarTodasAsEvidenciasRotaHandler(IEvidenciaRotaQuery evidenciaRota)
    {
        _evidenciaRota = evidenciaRota;
    }

    public async Task<ICollection<Dommain.Entities.EvidenciaRota>> Handler(BuscarTodasEvidenciasRotaCommands commands)
    {
        var evidencias = await _evidenciaRota.GetEvidenciasPagination(commands.IdRota, commands.Page, commands.PageSize);

        if(evidencias.Count == 0)
        {
            throw new Exception("Não há evidencias para ser mostrada");
        }

        return evidencias;
    }
}
