namespace APIRelatorios.Infra.Requets;

public class AneelService
{
    private readonly HttpClient _httpClient;

    public AneelService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> BuscaDeDadosBDGD()
    {
        // Aguardando URL Atualizada da api de dados da ANEEL
        var response = await _httpClient.GetAsync("");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}