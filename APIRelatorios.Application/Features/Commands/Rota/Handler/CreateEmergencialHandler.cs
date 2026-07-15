using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Contracts.Enum;
using APIRelatorios.Application.Exceptions.Integrations;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Dommain.Helpers;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateEmergencialHandler 
    : ICommandHandler<CreateEmergencialCommand, byte[]>
{
    private readonly IRotaQuery _rotaQuery;

    private readonly IEvidenciaRotaQuery _evidenciaRotaQuery;

    private readonly IApiDocxService _apiDocx;

    private readonly ILogger<CreateEmergencialCommand> _logger;

    public CreateEmergencialHandler(ILogger<CreateEmergencialCommand> logger, IEvidenciaRotaQuery evidenciaRotaQuery, IRotaQuery rotaQuery, IApiDocxService apiDocx)
    {
        _logger = logger;
        _evidenciaRotaQuery = evidenciaRotaQuery;
        _rotaQuery = rotaQuery;
        _apiDocx = apiDocx;
    }
    public async Task<byte[]> Handle(CreateEmergencialCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando Handler de Relatorio Emergencial");

        //Fazendo Buscas no banco de dados
        var rota = await _rotaQuery.BuscarRotaID(command.IdRota) ?? 
                    throw new RotaNotFoundException();

        var evidencias = await _evidenciaRotaQuery.GetEvidenciasUrgencia(command.IdRota) ?? 
                    throw new RotaNotFoundException();

        List<EvidenciaDocs> docs = new();

        _logger.LogInformation("Atribuindo valores da evidencia para evidenciaDocs");

        foreach (var evi in evidencias)
        {
            List<string> imagesMedium = new();

            foreach (var image in evi.Images)
            {
                imagesMedium.Add(image.OriginalUrl);
            }

            EvidenciaDocs docx = new EvidenciaDocs
            {
                DESC = evi.Descricao ?? "Obsservação Nula",
                ALI = evi.Alimentador ?? "Alimentador Nulo",
                CID = evi.Cidade ?? "Cidade Nula",
                images = imagesMedium
            };
            // Adicionando evidencia para dentro do dto
            docs.Add(docx);
        }

        _logger.LogInformation("Iniciando de Atribuir dados a rota");

        //Pegando datas inicial e final das evidencias
        var diasInt = evidencias.Select(x => x.Horario.Day);
        var mesInt = evidencias.First().Horario.Month;

        int minDay = diasInt.Min();
        int maxDay = diasInt.Max();

        List<int> data = new() { minDay };

        if (minDay != maxDay)
            data.Add(maxDay);

        EmergencialDTO emergencialDTO = new EmergencialDTO
        { 
            Conc = rota.Concessionarias.ToConc(),
            Mes = ((Meses)mesInt).ToMes(),
            Ano = DateTime.Now.Year.ToString(),
            Dias = data,
            Evidencias = docs,
        };

        _logger.LogInformation("terminando de Atribuir dados da rota");

        var resultBytes = await _apiDocx.emergencial(emergencialDTO);

        if (resultBytes == null || resultBytes.Length == 0)
        {
            throw new DocxApiException();
        }

        return resultBytes;
    }
}
