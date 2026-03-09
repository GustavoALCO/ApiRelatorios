using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Querys.Rota.Handler;

public class BuscarRotaIdHandler
{
    private readonly IRotaQuery _query;

    public BuscarRotaIdHandler(IRotaQuery query)
    {
        _query = query;
    }

    public async Task<RotaDTO> Handler(int id)
    {
        var rota = await _query.BuscarRotaID(id) ?? throw new Exception("Erro ao buscar rota");

        RotaDTO rotaDTO = new()
            {
            RotaId = rota.RotaId,
            NomeRota = rota.NomeRota,
            Alimentador = rota.Alimentador,
            DataInicio = rota.DataInicio.ToString("dd/MM/yyyy"),
            DataFinal = rota.DataFinal?.ToString("dd/MM/yyyy"),
        };
        
        return rotaDTO;
    }
}
