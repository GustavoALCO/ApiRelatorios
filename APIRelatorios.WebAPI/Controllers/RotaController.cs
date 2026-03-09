using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Features.Commands.Rota.Handler;
using APIRelatorios.Application.Features.Querys.Rota;
using APIRelatorios.Application.Features.Querys.Rota.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("Rotas")]
public class RotaController : ControllerBase
{
    private readonly AddFiscalRotaHandler _addFiscal;
    private readonly CreateRotaHandler _createRota;
    private readonly DeleteRotaHandler _deleteRota;
    private readonly RemoveFiscalRotaHandler _rmvFiscalRota;
    private readonly UpdateNomeRotaHandler _updateNomeRota;
    private readonly CreateRelatorioHandler _createRelatorio;
    private readonly BuscarRotaFiltersHandler _buscarRotaFilters;
    private readonly BuscarRotaIdHandler _buscarRotaIdHandler;

    public RotaController(
        AddFiscalRotaHandler addFiscal,
        CreateRotaHandler createRota,
        DeleteRotaHandler deleteRota,
        RemoveFiscalRotaHandler rmvFiscalRota,
        UpdateNomeRotaHandler updateNomeRota,
        CreateRelatorioHandler createRelatorio,
        BuscarRotaFiltersHandler buscarRotaFilters,
        BuscarRotaIdHandler buscarRotaIdHandler)
    {
        _addFiscal = addFiscal;
        _createRota = createRota;
        _deleteRota = deleteRota;
        _rmvFiscalRota = rmvFiscalRota;
        _updateNomeRota = updateNomeRota;
        _createRelatorio = createRelatorio;
        _buscarRotaFilters = buscarRotaFilters;
        _buscarRotaIdHandler = buscarRotaIdHandler;
    }

    //[authorize]
    [HttpGet]
    public async Task<IActionResult> BuscarPorFiltro(
         [FromQuery]BuscarRotaFiltersCommands command)
    {
        try
        {
            var rota = await _buscarRotaFilters.Handler(command);
            return Ok(rota);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno ao buscar rotas.\n{ex}");
        }
    }

    //[authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorFiltro(
        int id)
    {
        try
        {
            var rota = await _buscarRotaIdHandler.Handler(id);
            return Ok(rota);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno ao buscar rotas.\n{ex}");
        }
    }

    //[authorize]
    [HttpPatch("nome")]
    public async Task<IActionResult> UpdNome(
        [FromBody] UpdateNomeRotaCommand command)
    {
        try
        {
            await _updateNomeRota.Handler(command);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao atualizar rota.");
        }
    }

    //[authorize]
    [HttpPatch("AddFiscais")]
    public async Task<IActionResult> AddFiscais(
        [FromBody] AddFiscalRotaCommand command)
    {
        try
        {
            await _addFiscal.Handler(command);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao adicionar fiscais.");
        }
    }

    //[authorize]
    [HttpPatch("RemoveFiscais")]
    public async Task<IActionResult> RemoverFiscais(
        [FromBody] RemoveFiscalRotaCommand command)
    {
        try
        {
            await _rmvFiscalRota.Handler(command);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao remover fiscais.");
        }
    }

    //[authorize]
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] CreateRotaCommand command)
    {
        try
        {
            await _createRota.Handler(command);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao criar rota.");
        }
    }

    //[authorize]
    [HttpDelete("{rotaId}")]
    public async Task<IActionResult> Delete( int rotaId)
    {
        if (rotaId <= 0)
            return BadRequest("Id invalido");
        try
        {
            await _deleteRota.Handler(rotaId);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao deletar rota.");
        }
    }

    //[authorize]
    [HttpPost("CriarRelatorio")]
    public async Task<IActionResult> CriarRelatorio(
        [FromBody] CreateRelatorioWordCommand command)
    {
        try
        {
            var bytes = await _createRelatorio.Handler(command);

            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "Relatorio.docx"
            );
        }
        catch (Exception ex )
        {
            return StatusCode(500, ex.Message);
        }
    }
}
