using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace APIRelatorios.Infra.Requets;

public class ApiDocxService : IApiDocxService
{
    private readonly ILogger<ApiDocxService> _logger;

    private readonly HttpClient _httpClient;

    public ApiDocxService(ILogger<ApiDocxService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<byte[]> emergencial(EmergencialDTO dto)
    {
        _logger.LogInformation("Fazendo o request para a Api de Docx");

        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        _logger.LogInformation("Payload enviado para API Docx: {json}", json);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var zip = await _httpClient.PostAsync("http://api-docx:8000/gerar-doc", content) ?? throw new Exception("Servico de Relatorios Indisponivel");

        zip.EnsureSuccessStatusCode();

        return await zip.Content.ReadAsByteArrayAsync();
    }
}
