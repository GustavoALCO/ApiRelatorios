using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;

namespace APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;

public class BuscarEvidenciaPorIdHandler
{
    private readonly IEvidenciaRotaQuery _evidenciaRota;

    public BuscarEvidenciaPorIdHandler(IEvidenciaRotaQuery evidenciaRota)
    {
        _evidenciaRota = evidenciaRota;
    }

    public async Task<Dommain.Entities.EvidenciaRota> Handler(BuscarEvidenciaPorIDCommands commands)
    {
        var evidencias = await _evidenciaRota.GetImageId(commands.IdEvidencia);

        if (evidencias == null)
        {
            throw new Exception("Não existe Evidencia com esse ID");
        }

        return evidencias;
    }
}
