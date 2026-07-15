using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace APIRelatorios.Application.Features.Querys.Rota.Handler;

public class BuscarRotaFiltersHandler
    : IQueryHandler<
        BuscarRotaFiltersQuery,
        ICollection<RotaDTO>
    >
{
    private readonly IRotaQuery _rotaQuery;
    private readonly IUserQuery _userQuery;

    private readonly ILogger<BuscarRotaFiltersHandler> _logger;

    public BuscarRotaFiltersHandler(
        IRotaQuery rotaQuery,
        IUserQuery userQuery,
        ILogger<BuscarRotaFiltersHandler> logger)
    {
        _rotaQuery = rotaQuery;
        _userQuery = userQuery;
        _logger = logger;
    }

    public async Task<ICollection<RotaDTO>> Handle(
        BuscarRotaFiltersQuery commands,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Iniciando busca de rotas. " +
            "FiscalId: {FiscalId}, Nome: {Nome}, " +
            "DataInicial: {DataInicial}, DataFinal: {DataFinal}, " +
            "TipoFiscalizacao: {TipoFiscalizacao}, " +
            "Página: {Pagina}, Tamanho: {Tamanho}",
            commands.FiscalId,
            commands.Nome,
            commands.DataInicial,
            commands.DataFinal,
            commands.tipoFiscalizacao,
            commands.page,
            commands.pagesize
        );

        var fiscal = await _userQuery.BuscarFiscalId(
            commands.FiscalId
        );

        if (fiscal == null)
        {
            _logger.LogWarning(
                "Fiscal não encontrado. FiscalId: {FiscalId}",
                commands.FiscalId
            );

            throw new UserNotFoundException(
                commands.FiscalId
            );
        }

        _logger.LogInformation(
            "Fiscal encontrado. FiscalId: {FiscalId}, Nome: {Nome}",
            commands.FiscalId,
            fiscal.Name
        );

        var query = _rotaQuery.BuscarQuery();

        _logger.LogInformation(
            "Aplicando filtro de rotas vinculadas ao fiscal {FiscalId}",
            commands.FiscalId
        );

        query = query.Where(
            rota => rota.Fiscais.Any(
                fiscalRota =>
                    fiscalRota.UserId == commands.FiscalId
            )
        );

        if (!string.IsNullOrWhiteSpace(commands.Nome))
        {
            var nome = commands.Nome.Trim().ToUpper();

            _logger.LogInformation(
                "Aplicando filtro de nome: {Nome}",
                commands.Nome
            );

            query = query.Where(
                rota =>
                    rota.NomeRota != null &&
                    rota.NomeRota.ToUpper().Contains(nome)
            );
        }

        if (!string.IsNullOrWhiteSpace(commands.DataInicial) &&
            !string.IsNullOrWhiteSpace(commands.DataFinal))
        {
            var dataInicial = ConverterData(
                commands.DataInicial,
                nameof(commands.DataInicial)
            );

            var dataFinal = ConverterData(
                commands.DataFinal,
                nameof(commands.DataFinal)
            );

            if (dataFinal < dataInicial)
            {
                _logger.LogWarning(
                    "Intervalo de datas invertido. " +
                    "DataInicial: {DataInicial}, DataFinal: {DataFinal}",
                    dataInicial,
                    dataFinal
                );

                // Troca os valores corretamente.
                (dataInicial, dataFinal) =
                    (dataFinal, dataInicial);
            }

            var fimDoDiaFinal =
                dataFinal.AddDays(1).AddTicks(-1);

            _logger.LogInformation(
                "Aplicando período entre {DataInicial} e {DataFinal}",
                dataInicial,
                fimDoDiaFinal
            );

            query = query.Where(
                rota =>
                    rota.DataInicio >= dataInicial &&
                    rota.DataInicio <= fimDoDiaFinal
            );
        }
        else if (!string.IsNullOrWhiteSpace(commands.DataInicial))
        {
            var dataInicial = ConverterData(
                commands.DataInicial,
                nameof(commands.DataInicial)
            );

            var fimDoDia =
                dataInicial.AddDays(1).AddTicks(-1);

            _logger.LogInformation(
                "Aplicando DataInicial entre {Inicio} e {Fim}",
                dataInicial,
                fimDoDia
            );

            query = query.Where(
                rota =>
                    rota.DataInicio >= dataInicial &&
                    rota.DataInicio <= fimDoDia
            );
        }
        else if (!string.IsNullOrWhiteSpace(commands.DataFinal))
        {
            var dataFinal = ConverterData(
                commands.DataFinal,
                nameof(commands.DataFinal)
            );

            var fimDoDia =
                dataFinal.AddDays(1).AddTicks(-1);

            _logger.LogInformation(
                "Aplicando DataFinal entre {Inicio} e {Fim}",
                dataFinal,
                fimDoDia
            );

            query = query.Where(
                rota =>
                    rota.DataFinal >= dataFinal &&
                    rota.DataFinal <= fimDoDia
            );
        }

        // A condição correta é != null.
        if (commands.tipoFiscalizacao != null)
        {
            _logger.LogInformation(
                "Aplicando TipoFiscalizacao: {TipoFiscalizacao}",
                commands.tipoFiscalizacao
            );

            query = query.Where(
                rota =>
                    rota.TipoFiscalizacao ==
                    commands.tipoFiscalizacao.Value
            );
        }
        else
        {
            _logger.LogInformation(
                "Nenhum filtro de TipoFiscalizacao informado"
            );
        }

        var pagina = commands.page <= 0
            ? 1
            : commands.page;

        var tamanhoPagina = commands.pagesize <= 0
            ? 10
            : commands.pagesize;

        _logger.LogInformation(
            "Executando consulta. Página: {Pagina}, Tamanho: {Tamanho}",
            pagina,
            tamanhoPagina
        );

        var searchFilters =
            await _rotaQuery.BuscarRotaFiltros(
                query,
                pagina,
                tamanhoPagina
            );

        _logger.LogInformation(
            "Consulta concluída. Rotas encontradas: {Quantidade}",
            searchFilters.Count
        );

        var filtersDto = searchFilters
            .Select(
                rota => new RotaDTO
                {
                    RotaId = rota.RotaId,
                    Alimentador = rota.Alimentador,
                    DataFinal = rota.DataFinal?
                        .ToString("dd/MM/yyyy"),
                    DataInicio = rota.DataInicio
                        .ToString("dd/MM/yyyy"),
                    Concessionarias =
                        rota.Concessionarias,
                    NomeRota = rota.NomeRota
                        ?? "Nome não informado",
                    TipoFiscalizacao =
                        rota.TipoFiscalizacao
                }
            )
            .ToList();

        _logger.LogInformation(
            "Handler retornando {Quantidade} DTOs para o fiscal {FiscalId}",
            filtersDto.Count,
            commands.FiscalId
        );

        return filtersDto;
    }

    private static DateTime ConverterData(
        string data,
        string nomeParametro)
    {
        if (!DateTime.TryParseExact(
                data,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var resultado))
        {
            throw new ArgumentException(
                $"A data '{data}' é inválida. " +
                "Utilize o formato dd/MM/yyyy.",
                nomeParametro
            );
        }

        return DateTime.SpecifyKind(
            resultado,
            DateTimeKind.Utc
        );
    }
}