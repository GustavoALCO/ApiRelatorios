using APIRelatorios.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Infra.Requets;

public class BuscarByteImagemService : IBuscarByteImagemService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BuscarByteImagemService> _logger;

    public BuscarByteImagemService(
        HttpClient httpClient,
        ILogger<BuscarByteImagemService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<byte[]> BaixarImagemAsync(string imageUrl)
    {
        try
        {
            _logger.LogInformation(imageUrl);

            using var response = await _httpClient.GetAsync(imageUrl);

            _logger.LogInformation(
                "Status Code: {StatusCode}",
                response.StatusCode);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao baixar imagem: {Url}",
                imageUrl);

            throw;
        }
    }
}