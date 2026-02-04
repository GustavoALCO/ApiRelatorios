using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class RemoveFiscalRotaHandler
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    private readonly IValidateIds _validateids;

    public RemoveFiscalRotaHandler(IRotaCommands commands, IRotaQuery query, IValidateIds validateids)
    {
        _commands = commands;
        _query = query;
        _validateids = validateids;
    }

    public async Task Handler(RemoveFiscalRotaCommand rmfisc)
    {
        foreach (var fiscais in rmfisc.fiscaisId)
        {
            if (await _validateids.UserExisteAsync(fiscais) is false)
                throw new Exception("Lista de Id invalida");
        }

        var rota = await _query.BuscarRotaID(rmfisc.rotaId) 
            ?? throw new Exception("Erro Ao buscar Rota no banco de dados");


        foreach (var userId in rmfisc.fiscaisId)
        {
            await _commands.RemoverFiscalRota(userId ,rmfisc.rotaId);
        }

    }
}
