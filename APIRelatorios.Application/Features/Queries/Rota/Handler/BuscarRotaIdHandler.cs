using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Features.Queries.Rota;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Querys.Rota.Handler;

public class BuscarRotaIdHandler
    : IQueryHandler<BuscarRotaIdQuery, RotaDTO>
{
    private readonly IRotaQuery _query;

    public BuscarRotaIdHandler(IRotaQuery query)
    {
        _query = query;
    }

    public async Task<RotaDTO> Handle(BuscarRotaIdQuery query, CancellationToken cancellationToken)
    {
        var rota = await _query.BuscarRotaID(query.RotaId) 
                    ?? throw new RotaNotFoundException(query.RotaId);

        RotaDTO rotaDTO = new()
            {
            RotaId = rota.RotaId,
            NomeRota = rota.NomeRota ?? "Nome não Informado",
            Alimentador = rota.Alimentador,
            DataInicio = rota.DataInicio.ToString("dd/MM/yyyy"),
            DataFinal = rota.DataFinal?.ToString("dd/MM/yyyy"),
            TipoFiscalizacao = rota.TipoFiscalizacao,
            Concessionarias = rota.Concessionarias,
        };
        
        return rotaDTO;
    }
}
