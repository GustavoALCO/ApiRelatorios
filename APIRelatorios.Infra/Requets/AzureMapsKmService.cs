using APIRelatorios.Application.Settings;
using APIRelatorios.Dommain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace APIRelatorios.Infra.Requets;

public class AzureMapsKmService : IAzureMapsKmService
{
    private readonly HttpClient _httpClient;

    private readonly ILogger<AzureMapsKmService> _logger;

    private readonly AzureMapsSettings _azureMapsSettings;

    private readonly string _url;

    private readonly string _version;

    public AzureMapsKmService(ILogger<AzureMapsKmService> logger, HttpClient httpClient, IOptions<AzureMapsSettings> configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _url = "https://atlas.microsoft.com/route/directions/";
        _version = "json?api-version=1.0";
        _azureMapsSettings = configuration.Value;
    }

    public async Task<double?> BuscarDistanciaKM(List<(double, double)> coordenadas)
    {
        _logger.LogInformation("Iniciando busca da distância percorrida");

        var query = string.Join(":", coordenadas.Select(c => $"{c.Item1},{c.Item2}"));

        var url = $"{_url}{_version}" +
                  $"&subscription-key={_azureMapsSettings.AzureMapsApiKey}" +
                  $"&query={query}";

        _logger.LogInformation(url);

        try
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var json = System.Text.Json.JsonDocument.Parse(content);

            var distanceMeters = json
                .RootElement
                .GetProperty("routes")[0]
                .GetProperty("summary")
                .GetProperty("lengthInMeters")
                .GetDouble();

            _logger.LogInformation($"Distancia Percorrida em metros {distanceMeters}");

            // Converter para KM
            return distanceMeters / 1000;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao Buscar quilometros percorridos {ex}");

            return null;
        }
        
    }

}
