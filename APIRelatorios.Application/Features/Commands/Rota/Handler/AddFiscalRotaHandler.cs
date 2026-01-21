using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class AddFiscalRotaHandler
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    public AddFiscalRotaHandler(IRotaQuery query, IRotaCommands commands)
    {
        _query = query;
        _commands = commands;
    }
    public async Task Handler(AddFiscalRotaCommand add)
    {
        var rota = await _query.BuscarRotaID(add.rotaId) ?? 
            throw new Exception("Erro ao Encontrar Rota no Banco de dados");

        foreach (var fiscalAdd in add.FiscaisId)
        {
            rota.AdicionarFiscal(fiscalAdd);
        }

        await _commands.UpdateRotaAsync(rota);
    }
}
