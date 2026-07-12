using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Domain.Interfaces.Amostra;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Queries.Amostra.Handler;

public class BuscarTodasAmostrasHandler : IQueryHandler<BuscarTodasAmostrasQuery, IEnumerable<AmostraDTO>>
{
    private readonly IAmostraQuery _query;
    private readonly ILogger<BuscarTodasAmostrasHandler> _logger;

    public BuscarTodasAmostrasHandler(ILogger<BuscarTodasAmostrasHandler> logger, IAmostraQuery query)
    {
        _logger = logger;
        _query = query;
    }

    public async Task<IEnumerable<AmostraDTO>> Handle(BuscarTodasAmostrasQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando todas as amostras para a rota: {RotaId}", query.idrota);

        var amostras = await _query.GetAmostraById(query.idrota);

        _logger.LogInformation("Amostras encontradas: {Count}", amostras.Count());

        return amostras.Select(amostra => new AmostraDTO
        {
            Id = amostra.Id,
            RotaId = amostra.RotaId,
            SeqISA = amostra.SeqISA,
            SeqBaseFisica = amostra.SeqBaseFisica,
            VlrBase = amostra.VlrBase,
            DescricaoTUC = amostra.DescricaoTUC,
            DescricaoTec = amostra.DescricaoTec,
            ODIEngenharia = amostra.ODIEngenharia,
            Instalacao = amostra.Instalacao,
            Endereco = amostra.Endereco,
            Municipio = amostra.Municipio,
            Latitude = amostra.Latitude,
            Longitude = amostra.Longitude,
            TUC1 = amostra.TUC1,
            TUC2 = amostra.TUC2,
            TUC3 = amostra.TUC3,
            TUC4 = amostra.TUC4,
            TUC5 = amostra.TUC5,
            TUC6 = amostra.TUC6,
            NumSerie = amostra.NumSerie,
            PosicaoOperativa = amostra.PosicaoOperativa,
            Equipamento = amostra.Equipamento,
            DataFabricacao = amostra.DataFabricacao,
            Observacao = amostra.Observacao
        });
    }
}
