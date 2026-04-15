using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;
using APIRelatorios.Dommain.Interfaces.Rota;

namespace APIRelatorios.Application.Features.Commands.RotaRetorno.Handler;

public class CreateRotaRetornoHandler
{

    private readonly IRotaQuery _rotaQuery;

    private readonly IRotaCommands _rotaCommands;

    public CreateRotaRetornoHandler(IRotaCommands rotaCommands, IRotaQuery rotaQuery)
    {
        _rotaCommands = rotaCommands;
        _rotaQuery = rotaQuery;
    }

    public async Task Handler(CreateRotaRetornoCommand command)
    {
        var rotaoriginal = await _rotaQuery.BuscarRotaID(command.rotaIdOriginal) ?? 
            throw new Exception("Erro ao encontrar Rota Existente");

        if (((int)rotaoriginal.Concessionarias) > ((int)Concessionarias.COOPERLUZ_SP) ||
            rotaoriginal.DataFinal == null ||
            rotaoriginal.Alimentador == null)
        {
            throw new Exception("Rota com Informações faltando");
        }

       var data =DateTime.UtcNow.AddHours(-3);

        Dommain.Entities.RotaRetorno rotaRetorno = 
            new
            (
            command.rotaId,
            $"{rotaoriginal.NomeRota} Retorno {data.ToString("dd/MM/yyyy")}",
            rotaoriginal.Concessionarias,
            rotaoriginal.Alimentador,
            data
            );

        await _rotaCommands.CreateRotaAsync(rotaRetorno);

        return;
    }
}
