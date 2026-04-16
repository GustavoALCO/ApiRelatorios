using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class FinalizarRotaHandler
{

    private readonly ILogger<FinalizarRotaHandler> _logger;

    private readonly IEvidenciaRotaQuery _eviQuery;

    private readonly IRotaQuery _query;

    private readonly IBuscarDistanciaCordenadas _buscarKm;

    private readonly IRotaCommands _commands;

    public FinalizarRotaHandler(IRotaCommands commands, IRotaQuery query, ILogger<FinalizarRotaHandler> logger, IEvidenciaRotaQuery eviQuery, IBuscarDistanciaCordenadas buscarKm)
    {
        _commands = commands;
        _query = query;
        _logger = logger;
        _eviQuery = eviQuery;
        _buscarKm = buscarKm;
    }

    public async Task Handler(FinalizarRotaCommand command)
    {
        _logger.LogInformation("Fazendo buscas de RotaId");
        var rota = await _query.BuscarRotaID(command.RotaId) ?? 
                        throw new Exception("Não foi possicel encontrar a Rota");

        _logger.LogInformation("Verificando se Rota tem Evidencias");
        var evidencias = await _eviQuery.GetEvidenciaAsync(command.RotaId) ?? 
                        throw new Exception("Não é possivel finalizar rota sem evidendencias");

        var numeroEvidencias = evidencias.Count();

        if (numeroEvidencias < 10)
            throw new Exception("É necessario ao menos 10 Evidencias");

        List<List<EvidenciaRota>> blocoRota = new();
        List<EvidenciaRota> blocoAtual = new();

        _logger.LogInformation("Montando Blocos para Consultas");
        if(blocoAtual.Count == 20)
        {
            blocoRota.Add(blocoAtual);
            blocoAtual.Clear();
        }
        for (int i = 0; i < evidencias.Count - 1; i++)
        {
            var atual = evidencias[i];
            var proximo = evidencias[i + 1];

            blocoAtual.Add(atual);

            var diff = proximo.Horario - atual.Horario;

            if (diff.TotalMinutes >= 40)
                break;
        }

        double Km = 0.0;

        foreach (var b in blocoRota)
        {
            List<(double, double)> cordenadas = new();

            _logger.LogInformation("Atribuindo valores a lista para cordenadas");
            foreach (var cor in evidencias)
            {
                cordenadas.Add((cor.Latitude, cor.Longitude));
            }

            _logger.LogInformation("Iniciando loop para buscar os Km percorridos");
            for (int i = 0; i < cordenadas.Count; i += 20)
            {
                var batch = cordenadas
                    .Skip(i == 0 ? i : i - 1) // repete o último ponto
                    .Take(20)
                    .ToList();

                Km += await _buscarKm.BuscarDistanciaKM(batch) ??
                                throw new Exception("objeto nulo"); ;
            }

            rota.finalizandoRota(command.DataFinal, Km);
        }
        

        _logger.LogInformation("Fazendo Update no Banco de dados");
        await _commands.UpdateRotaAsync(rota);
    }
}
