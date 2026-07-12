using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Features.Commands.Amostra;
using APIRelatorios.Application.Features.Queries.Amostra;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("Amostra")]
public class AmostraController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public AmostraController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarPorId([FromQuery] int id)
    {
        var query = new BuscarAmostraPorIdQuery(id);

        var resultado = await _dispatcher.Query<
            BuscarAmostraPorIdQuery,
            AmostraDTO>(query);

        return Ok(resultado);
    }

    [Authorize]
    [HttpGet("TodasAmostras")]
    public async Task<IActionResult> BuscarTodosPorRotaId([FromQuery] Guid idrota)
    {
        var query = new BuscarTodasAmostrasQuery(idrota);

        var resultado = await _dispatcher.Query<BuscarTodasAmostrasQuery, IEnumerable<AmostraDTO>>(query);

        return Ok(resultado);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CriarDadosAmostra([FromQuery]Guid id, [FromForm] IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest("Arquivo não enviado.");

        using var reader = new StreamReader(arquivo.OpenReadStream());

        var config = new CsvConfiguration(new CultureInfo("pt-BR"))
        {
            Delimiter = ";",
            HasHeaderRecord = true,
            BadDataFound = null,
            MissingFieldFound = null
        };

        using var csv = new CsvReader(reader, config);

        var registros = csv.GetRecords<AmostraCsvDto>();

        foreach (var item in registros)
        {
            var command = new CreateAmostraCommand(
                id,
                item.SeqISA,
                item.SeqBaseFisica,
                item.VlrBase.ToString(),
                item.DescricaoTUC,
                item.DescricaoTec,
                item.ODIEngenharia,
                item.Instalacao,
                item.Endereco,
                item.Municipio,
                item.Latitude,
                item.Longitude,
                item.TUC1,
                item.TUC2,
                item.TUC3,
                item.TUC4,
                item.TUC5,
                item.TUC6,
                item.NumSerie,
                item.PosicaoOperativa,
                item.Equipamento
            );

            await _dispatcher.Send(command);
        }

        return Ok("Arquivo processado com sucesso");
    }

    [Authorize]
    [HttpPatch]
    public async Task <IActionResult> AlterarVariaveis([FromBody] UpdateAmostraCommand updateAmostra)
    {
        await _dispatcher.Send(updateAmostra);

        return Ok();
    }
}