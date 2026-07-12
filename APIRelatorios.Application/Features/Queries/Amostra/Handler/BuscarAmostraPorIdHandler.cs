using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Domain.Interfaces.Amostra;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Queries.Amostra.Handler;

public class BuscarAmostraPorIdHandler : IQueryHandler<BuscarAmostraPorIdQuery, AmostraDTO>
{
    private readonly ILogger<BuscarAmostraPorIdHandler> _logger;

    private readonly IAmostraQuery _amostraquery;

    public BuscarAmostraPorIdHandler(IAmostraQuery amostraquery, ILogger<BuscarAmostraPorIdHandler> logger)
    {
        _amostraquery = amostraquery;
        _logger = logger;
    }

    public async Task<AmostraDTO> Handle(BuscarAmostraPorIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando amostra com ID: {Id}", query.id);

        var amostra = await _amostraquery.GetAmostraId(query.id) ?? throw new AmostraNotFoundException();

        _logger.LogInformation("Amostra encontrada: {@Amostra}", amostra);

        var amostraDTO = new AmostraDTO
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
            Observacao = amostra.Observacao,
            Fotos = amostra.Fotos
        };

        return amostraDTO;
    }
}
