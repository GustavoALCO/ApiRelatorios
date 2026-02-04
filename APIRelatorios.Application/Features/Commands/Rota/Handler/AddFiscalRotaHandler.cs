using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class AddFiscalRotaHandler
{
    private readonly IRotaQuery _query;

    private readonly IRotaCommands _commands;

    private readonly IValidateIds _validateids;

    public AddFiscalRotaHandler(IRotaQuery query, IRotaCommands commands, IValidateIds validateids)
    {
        _query = query;
        _commands = commands;
        _validateids = validateids;
    }
    public async Task Handler(AddFiscalRotaCommand add)
    {
        foreach(var fiscais in add.FiscaisId)
        {
            if (await _validateids.UserExisteAsync(fiscais) is false)
                throw new Exception("Lista de Id invalida");
        }

        var rota = await _query.BuscarRotaID(add.rotaId) ?? 
            throw new Exception("Erro ao Encontrar Rota no Banco de dados");

        foreach (var fiscalAdd in add.FiscaisId)
        {
            UsuarioRota user = new()
            { RotaID = add.rotaId,
            UserID = fiscalAdd};

            await _commands.AdicionarFiscalRota(user);
        }

        
    }
}
