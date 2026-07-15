using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Features.Querys.Rota;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Queries.Rota.Handler;

public class BuscarRotaAmostraCommands : IQueryHandler<BuscarRotaAmostraQuery, ICollection<RotaDTO>>
{
    private readonly IRotaQuery _rotaQuery;

    private readonly IUserQuery _userQuery;

    private readonly ILogger<BuscarRotaAmostraCommands> _logger;

    public BuscarRotaAmostraCommands(IUserQuery userQuery, IRotaQuery rotaQuery, ILogger<BuscarRotaAmostraCommands> logger)
    {
        _userQuery = userQuery;
        _rotaQuery = rotaQuery;
        _logger = logger;
    }

    public async Task<ICollection<RotaDTO>> Handle(BuscarRotaAmostraQuery query, CancellationToken cancellationToken)
    {
        var user = await _userQuery.BuscarFiscalId(query.idUser) ?? throw new UserNotFoundException(query.idUser);

        _logger.LogInformation($"Usuario - {user.Name}-{user.LastName} Buscando Amostras");

        var searchFilters = await _rotaQuery.GetAmostra() ?? throw new RotaNotFoundException();

        _logger.LogInformation($"Foram encontradas {searchFilters.Count}");

        List<RotaDTO> filtersdto = new();

        foreach (var filters in searchFilters)
        {
            RotaDTO dto = new RotaDTO()
            {
                RotaId = filters.RotaId,
                Alimentador = filters.Alimentador,
                DataFinal = filters.DataFinal?.ToString("dd/MM/yyyy"),
                DataInicio = filters.DataInicio.ToString("dd/MM/yyyy"),
                Concessionarias = filters.Concessionarias,
                NomeRota = filters.NomeRota ?? "Nome não informado",
                TipoFiscalizacao = filters.TipoFiscalizacao
            };

            filtersdto.Add(dto);
        }

        return filtersdto;
    }
}
