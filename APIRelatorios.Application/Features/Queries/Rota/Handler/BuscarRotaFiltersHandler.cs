using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using System.Globalization;

namespace APIRelatorios.Application.Features.Querys.Rota.Handler;

public class BuscarRotaFiltersHandler
    : IQueryHandler<BuscarRotaFiltersQuery, ICollection<RotaDTO>>
{
    private readonly IRotaQuery _rotaQuery;

    private readonly IUserQuery _userQuery;

    public BuscarRotaFiltersHandler(IRotaQuery rotaQuery, IUserQuery userQuery)
    {
        _rotaQuery = rotaQuery;
        _userQuery = userQuery;
    }

    public async Task<ICollection<RotaDTO>> Handle(BuscarRotaFiltersQuery commands, CancellationToken cancellationToken)
    {
        //valida se o fiscal é existente
        var fiscal = await _userQuery.BuscarFiscalId(commands.FiscalId) ?? throw new UserNotFoundException(commands.FiscalId);

        //Buscar IQueryable para buscar filtros
        var query = _rotaQuery.BuscarQuery();

        //Passa o parametro para buscar apenas rotas que o usuario esta incluido.
        query = query.Where(x => x.Fiscais.Any(f => f.UserId == commands.FiscalId));


        //Verifica se o valor de nome não é nulo
        if (!string.IsNullOrEmpty(commands.Nome))
        {
            //Passa o parametro do nome para a busca de filtros
            query = query.Where(x => x.NomeRota.ToUpper().Contains(commands.Nome.ToUpper()));
        }

        //Verifica Se a data Inicial é nula
        if (!string.IsNullOrEmpty(commands.DataInicial) && !string.IsNullOrEmpty(commands.DataFinal))
        {
            var dataInicial = DateTime.SpecifyKind(
                DateTime.ParseExact(commands.DataInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTimeKind.Utc
            );
    
            var dataFinal = DateTime.SpecifyKind(
                DateTime.ParseExact(commands.DataFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTimeKind.Utc
            );

            if (dataFinal < dataInicial)
            {
                dataFinal = dataInicial;
                dataInicial = dataFinal;
            }

            query = query.Where(x => x.DataFinal >= dataInicial && x.DataFinal <= dataFinal);
        }
        //Aplica o filtro apenas se o valor de data Inicial for valido
        else if (!string.IsNullOrEmpty(commands.DataInicial))
        {

            var dataInicial = DateTime.SpecifyKind(
                DateTime.ParseExact(commands.DataInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTimeKind.Utc
            );

            query = query.Where(x => x.DataInicio == dataInicial);
        }
        //Aplica o filtro apenas se o valor de data Finalw for valido
        else if (!string.IsNullOrEmpty(commands.DataFinal))
        {
            var dataFinal = DateTime.SpecifyKind(
                DateTime.ParseExact(commands.DataFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTimeKind.Utc
            );
            var dataFinalFim = dataFinal.AddDays(1).AddTicks(-1);

            query = query.Where(x => x.DataFinal >= dataFinal && x.DataFinal <= dataFinalFim);
            
        }

        if (commands.tipoFiscalizacao == null)
        {
            query = query.Where(x => x.TipoFiscalizacao == commands.tipoFiscalizacao);
        }

        var searchFilters = await _rotaQuery.BuscarRotaFiltros(query, commands.page, commands.pagesize);

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
