using Castle.Core.Logging;

namespace APIRelatorios.Testes.Infra.Requests;

public class RequestBDGD
{
    private readonly HttpClient _httpClient;

    private string _url = "";

    private string _query = "";

    public RequestBDGD(HttpClient httpClient)
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