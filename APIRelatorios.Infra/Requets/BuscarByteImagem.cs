using APIRelatorios.Application.Interfaces;

namespace APIRelatorios.Infra.Requets;

public class BuscarByteImagem : IBuscarByteImagem
{
    public async Task<byte[]> BaixarImagemAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();

        var response = await httpClient.GetAsync(imageUrl);

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Erro ao baixar imagem do Blob Storage.");

        return await response.Content.ReadAsByteArrayAsync();
    }
}
