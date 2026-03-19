using APIRelatorios.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Infra.Requets;

public class BuscarByteImagem : IBuscarByteImagem
{
    private readonly ILogger<BuscarByteImagem> _logger;

    public BuscarByteImagem(ILogger<BuscarByteImagem> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> BaixarImagemAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();

        _logger.LogInformation(imageUrl);

        var response = await httpClient.GetAsync(imageUrl);

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Erro ao baixar imagem do Blob Storage.");

        return await response.Content.ReadAsByteArrayAsync();
    }
}
