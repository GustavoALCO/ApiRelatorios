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
        var response = await _httpClient.GetAsync("http://aneel-service:8000/dados");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}