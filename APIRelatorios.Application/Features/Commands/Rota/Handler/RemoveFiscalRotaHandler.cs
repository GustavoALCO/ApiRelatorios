using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class RemoveFiscalRotaHandler
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    public RemoveFiscalRotaHandler(IRotaCommands commands, IRotaQuery query)
    {
        _commands = commands;
        _query = query;
    }

    public async Task Handler(RemoveFiscalRotaCommand rmfisc)
    {
        var rota = await _query.BuscarRotaID(rmfisc.rotaId) 
            ?? throw new Exception("Erro Ao buscar Rota no banco de dados");

        foreach(var fiscais in rmfisc.fiscaisId)
        {
            rota.RemoverFiscal(fiscais);
        }

        await _commands.UpdateRotaAsync(rota);
    }
}
