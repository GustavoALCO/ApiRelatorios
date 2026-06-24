using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Features.Commands.Rota.Handler;
using APIRelatorios.Application.Features.Queries.Rota;
using APIRelatorios.Application.Features.Querys.Rota;
using APIRelatorios.Application.Features.Querys.Rota.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("Rotas")]
public class RotaController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

     public RotaController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> BuscarPorFiltro(
         [FromQuery]BuscarRotaFiltersQuery command)
    {

        var rota = await _dispatcher.Query<BuscarRotaFiltersQuery, ICollection<RotaDTO>>(command);
        return Ok(rota);

    }

    [Authorize]
    [HttpGet("id")]
    public async Task<IActionResult> BuscarPorFiltro(
        [FromQuery] BuscarRotaIdQuery id)
    {

        var rota = await _dispatcher.Query<BuscarRotaIdQuery, RotaDTO>(id);
        return Ok(rota);
    }

    [Authorize]
    [HttpPatch("nome")]
    public async Task<IActionResult> UpdNome(
        [FromBody] UpdateNomeRotaCommand command)
    {

        await _dispatcher.Send<UpdateNomeRotaCommand>(command);
        return Ok();

    }

    [Authorize]
    [HttpPatch("AddFiscais")]
    public async Task<IActionResult> AddFiscais(
        [FromBody] AddFiscalRotaCommand command)
    {

        await _dispatcher.Send<AddFiscalRotaCommand>(command);
        return Ok();
    }

    [Authorize]
    [HttpPatch("RemoveFiscais")]
    public async Task<IActionResult> RemoverFiscais(
        [FromBody] RemoveFiscalRotaCommand command)
    {

        await _dispatcher.Send<RemoveFiscalRotaCommand>(command);
        return Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] CreateRotaCommand command)
    {

        await _dispatcher.Send<CreateRotaCommand>(command);
        return Ok();

    }

    [Authorize]
    [HttpDelete("{rotaId}")]
    public async Task<IActionResult> Delete( DeleteRotaCommand rotaId)
    {

        await _dispatcher.Send<DeleteRotaCommand>(rotaId);
        return Ok();

    }

    [Authorize]
    [HttpPost("CriarRelatorio")]
    public async Task<IActionResult> CriarRelatorio(
        [FromBody] CreateRelatorioWordCommand command)
    {
        var bytes = await _dispatcher.Send<CreateRelatorioWordCommand, byte[]>(command);

        return File(
                    bytes,
                    "application/zip",
                    "Relatorio.zip"
                );

    }

    [Authorize]
    [HttpPost("CriarEmergencial")]
    public async Task<IActionResult> CriarEmergencial(
        [FromBody] CreateEmergencialCommand command)
    {

        var bytes = await _dispatcher.Send<CreateEmergencialCommand, byte[]>(command);

        return File(
                    bytes,
                    "application/zip",
                    "Emergencial.zip"
                );
    }

    [Authorize]
    [HttpPatch("FinalizarRota")]
    public async Task<IActionResult> FinalizarRota(
        [FromBody] FinalizarRotaCommand command)
    {

        await _dispatcher.Send<FinalizarRotaCommand>(command);

        return Ok();

    }
}
