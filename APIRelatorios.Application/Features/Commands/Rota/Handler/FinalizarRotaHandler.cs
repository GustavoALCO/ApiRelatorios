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

    private readonly IAzureMapsKmService _buscarKm;

    private readonly IRotaCommands _commands;

    public FinalizarRotaHandler(IRotaCommands commands, IRotaQuery query, ILogger<FinalizarRotaHandler> logger, IEvidenciaRotaQuery eviQuery, IAzureMapsKmService buscarKm)
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

        var rota = await _query.BuscarRotaID(command.RotaId)
            ?? throw new Exception("Não foi possível encontrar a Rota");

        var evidencias = await _eviQuery.GetEvidenciaAsync(command.RotaId)
            ?? throw new Exception("Não é possível finalizar rota sem evidências");

        var numeroEvidencias = evidencias.Count();

        if (numeroEvidencias < 10)
            throw new Exception("É necessário ao menos 10 evidências");

        _logger.LogInformation("Ordenando evidências");

        var ordenadas = evidencias
            .OrderBy(x => x.Horario)
            .ToList();

        List<List<EvidenciaRota>> blocos = new();
        List<EvidenciaRota> blocoAtual = new();

        _logger.LogInformation("Montando blocos por intervalo de 40 minutos");

        for (int i = 0; i < ordenadas.Count; i++)
        {
            var atual = ordenadas[i];

            blocoAtual.Add(atual);

            bool quebraTempo = false;

            if (i < ordenadas.Count - 1)
            {
                var proximo = ordenadas[i + 1];
                var diff = proximo.Horario - atual.Horario;

                if (diff.TotalMinutes >= 40)
                    quebraTempo = true;
            }

            if (blocoAtual.Count == 20 || quebraTempo || i == ordenadas.Count - 1)
            {
                blocos.Add(new List<EvidenciaRota>(blocoAtual));
                blocoAtual.Clear();
            }
        }

        double kmTotal = 0.0;

        _logger.LogInformation("Calculando KM por blocos");

        foreach (var bloco in blocos)
        {
            var coordenadas = bloco
                .Select(x => (x.Latitude, x.Longitude))
                .ToList();

            for (int i = 0; i < coordenadas.Count - 1; i += 19)
            {
                var batch = coordenadas
                    .Skip(i)
                    .Take(20)
                    .ToList();

                if (batch.Count < 2)
                    continue;

                var km = await _buscarKm.BuscarDistanciaKM(batch);

                if (km.HasValue)
                    kmTotal += km.Value;
            }
        }

        rota.finalizandoRota(DateTime.UtcNow.AddHours(-3), kmTotal);

        _logger.LogInformation("KM total calculado: {Km}", kmTotal);

        _logger.LogInformation("Fazendo update no banco");

        await _commands.UpdateRotaAsync(rota);
    }
}
