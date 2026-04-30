using APIRelatorios.Application.Settings;
using APIRelatorios.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.Json;

namespace APIRelatorios.Infra.Requets;

public class AzureMapsEnderecoService : IAzureMapsEnderecoService
{
    private readonly HttpClient _httpClient;

    private readonly ILogger<AzureMapsEnderecoService> _logger;

    private readonly AzureMapsSettings _azureMapsSettings;

    private readonly string _url;

    private readonly string _version;

    public AzureMapsEnderecoService(ILogger<AzureMapsEnderecoService> logger, HttpClient httpClient, IOptions<AzureMapsSettings> configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _url = "https://atlas.microsoft.com/search/address/reverse/";
        _version = "json?api-version=1.0";
        _azureMapsSettings = configuration.Value;
    }

    public async Task<(string, string)?> BuscarNomeRua(double lat, double lng)
    {
        _logger.LogInformation("Iniciando busca do nome da cidade e rua");

        var url = $"{_url}{_version}" +
                  $"&subscription-key={_azureMapsSettings.AzureMapsApiKey}" +
                  $"&query={lat.ToString(CultureInfo.InvariantCulture)},{lng.ToString(CultureInfo.InvariantCulture)}";

        _logger.LogInformation(url);

        try
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var json = JsonDocument.Parse(content);

            var root = json.RootElement;

            if (!root.TryGetProperty("addresses", out var addresses) || addresses.GetArrayLength() == 0)
            {
                _logger.LogWarning("Nenhum endereço encontrado");
                return null;
            }

            var address = addresses[0].GetProperty("address");

            var rua = address.TryGetProperty("streetName", out var street)
                ? street.GetString()
                : "";

            var numero = address.TryGetProperty("streetNumber", out var num)
                ? num.GetString()
                : "";

            var cidade = address.TryGetProperty("municipality", out var city)
                ? city.GetString()
                : "";

            var enderecoFormatado = $"{rua} {numero}".Trim();

            _logger.LogInformation($"Busca retornada com {enderecoFormatado}, {cidade}");

            return (enderecoFormatado, cidade);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao buscar endereço: {ex}");
            return null;
        }
    }
}
