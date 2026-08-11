using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Domain.Entities;
using APIRelatorios.Domain.Interfaces.Amostra;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Queries.Amostra.Handler;

public class BuscarJsonAmostrasColetadasHandler : IQueryHandler<BuscarJsonAmostrasColetadasQuery, IEnumerable<AmostraDTO>>
{
    private readonly IAmostraQuery _amostraQuery;

    private readonly ILogger<BuscarJsonAmostrasColetadasHandler> _logger;

    public BuscarJsonAmostrasColetadasHandler(ILogger<BuscarJsonAmostrasColetadasHandler> logger, IAmostraQuery amostraQuery)
    {
        _logger = logger;
        _amostraQuery = amostraQuery;
    }

    public async Task<IEnumerable<AmostraDTO>> Handle(BuscarJsonAmostrasColetadasQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando a execução do handler BuscarJsonAmostrasColetadasHandler");

        var amostras = await _amostraQuery.GetAmostraCheck(query.idrota);

        _logger.LogInformation("Foi encontradas {0} amostras", amostras.Count());

        _logger.LogInformation("Iniciando Processo de Mapear amostras para DTOs");

        ICollection<AmostraDTO> amostrasDTO = [];

        int count = 1;

        foreach (var item in amostras)
        {
            _logger.LogInformation("Mapeando amostra com Id: {0}", item.Id);
            count++;

            var amostraDTO = new AmostraDTO
            {
                Id = item.Id,
                RotaId = item.RotaId,
                SeqISA = item.SeqISA,
                SeqBaseFisica = item.SeqBaseFisica,
                VlrBase = item.VlrBase,
                DescricaoTUC = item.DescricaoTUC,
                DescricaoTec = item.DescricaoTec,
                ODIEngenharia = item.ODIEngenharia,
                Instalacao = item.Instalacao,
                Endereco = item.Endereco,
                Municipio = item.Municipio,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                TUC1 = item.TUC1,
                TUC2 = item.TUC2,
                TUC3 = item.TUC3,
                TUC4 = item.TUC4,
                TUC5 = item.TUC5,
                TUC6 = item.TUC6,
                NumSerie = item.NumSerie,
                PosicaoOperativa = item.PosicaoOperativa,
                Equipamento = item.Equipamento,
                DataFabricacao = item.DataFabricacao,
                Observacao = item.Observacao,
                Fotos = item.Fotos,
                Sincronizado = item.Sincronizado,
            };

            amostrasDTO.Add(amostraDTO);
        }

        return amostrasDTO;
    }
}
