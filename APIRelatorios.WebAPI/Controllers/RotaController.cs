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
        try
        {
            var rota = await _dispatcher.Query<BuscarRotaFiltersQuery, ICollection<RotaDTO>>(command);
            return Ok(rota);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno ao buscar rotas.\n{ex}");
        }
    }

    [Authorize]
    [HttpGet("id")]
    public async Task<IActionResult> BuscarPorFiltro(
        [FromQuery] BuscarRotaIdQuery id)
    {
        try
        {
            var rota = await _dispatcher.Query<BuscarRotaIdQuery, RotaDTO>(id);
            return Ok(rota);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno ao buscar rotas.\n{ex}");
        }
    }

    [Authorize]
    [HttpPatch("nome")]
    public async Task<IActionResult> UpdNome(
        [FromBody] UpdateNomeRotaCommand command)
    {
        try
        {
            await _dispatcher.Send<UpdateNomeRotaCommand>(command);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao atualizar rota.");
        }
    }

    [Authorize]
    [HttpPatch("AddFiscais")]
    public async Task<IActionResult> AddFiscais(
        [FromBody] AddFiscalRotaCommand command)
    {
        try
        {
            await _dispatcher.Send<AddFiscalRotaCommand>(command);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao adicionar fiscais.");
        }
    }

    [Authorize]
    [HttpPatch("RemoveFiscais")]
    public async Task<IActionResult> RemoverFiscais(
        [FromBody] RemoveFiscalRotaCommand command)
    {
        try
        {
            await _dispatcher.Send<RemoveFiscalRotaCommand>(command);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao remover fiscais.");
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] CreateRotaCommand command)
    {
        try
        {
            await _dispatcher.Send<CreateRotaCommand>(command);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao criar rota.");
        }
    }

    [Authorize]
    [HttpDelete("{rotaId}")]
    public async Task<IActionResult> Delete( DeleteRotaCommand rotaId)
    {
        try
        {
            await _dispatcher.Send<DeleteRotaCommand>(rotaId);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao deletar rota.");
        }
    }

    [Authorize]
    [HttpPost("CriarRelatorio")]
    public async Task<IActionResult> CriarRelatorio(
        [FromBody] CreateRelatorioWordCommand command)
    {
        try
        {
            var bytes = await _dispatcher.Send<CreateRelatorioWordCommand, byte[]>(command);

            return File(
                        bytes,
                        "application/zip",
                        "Relatorio.zip"
                    );
        }
        catch (Exception ex )
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("CriarEmergencial")]
    public async Task<IActionResult> CriarEmergencial(
        [FromBody] CreateEmergencialCommand command)
    {
        try
        {
            var bytes = await _dispatcher.Send<CreateEmergencialCommand, byte[]>(command);

            return File(
                        bytes,
                        "application/zip",
                        "Emergencial.zip"
                    );
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPatch("FinalizarRota")]
    public async Task<IActionResult> FinalizarRota(
        [FromBody] FinalizarRotaCommand command)
    {
        try
        {
            await _dispatcher.Send<FinalizarRotaCommand>(command);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
