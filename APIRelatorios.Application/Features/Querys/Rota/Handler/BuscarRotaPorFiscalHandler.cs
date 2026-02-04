using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Querys.Rota.Handler;

public class BuscarRotaPorFiscalHandler
{
    private readonly IRotaQuery _rotaQuery;

    public BuscarRotaPorFiscalHandler(IRotaQuery rotaQuery)
    {
        _rotaQuery = rotaQuery;
    }

    public async Task<ICollection<RotaDTO>> Handler(BuscarRotaPorFiscalCommands commands)
    {
        var rotas = await _rotaQuery.BuscarRotasPorFiscal(commands.IdFiscal,commands.Page, commands.PageSize ) 
            ?? throw new Exception("Não há Rotas para mostrar do usuario");

        ICollection<RotaDTO> ret = [];

        foreach (var rota in rotas)
        {
            RotaDTO rotaDTO = new()
            {
                RotaId = rota.RotaId,
                DataFinal = rota.DataFinal,
                DataInicio = rota.DataInicio,
                NomeRota = rota.NomeRota ?? "Nome Incompleto",
            };

            ret.Add(rotaDTO);
        }

        return ret;
        
    }
}
