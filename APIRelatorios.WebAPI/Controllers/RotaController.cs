using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Features.Commands.Rota.Handler;
using APIRelatorios.Application.Features.Querys.Rota;
using APIRelatorios.Application.Features.Querys.Rota.Handler;
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
    private readonly BuscarRotaPorFiscalHandler _buscarRotaPorFiscal;

    public RotaController(
        AddFiscalRotaHandler addFiscal,
        CreateRotaHandler createRota,
        DeleteRotaHandler deleteRota,
        RemoveFiscalRotaHandler rmvFiscalRota,
        UpdateNomeRotaHandler updateNomeRota,
        CreateRelatorioHandler createRelatorio,
        BuscarRotaPorFiscalHandler buscarRotaPorFiscal)
    {
        _addFiscal = addFiscal;
        _createRota = createRota;
        _deleteRota = deleteRota;
        _rmvFiscalRota = rmvFiscalRota;
        _updateNomeRota = updateNomeRota;
        _createRelatorio = createRelatorio;
        _buscarRotaPorFiscal = buscarRotaPorFiscal;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarPorFiscais(
        [FromQuery] BuscarRotaPorFiscalCommands command)
    {
        try
        {
            var rota = await _buscarRotaPorFiscal.Handler(command);
            return Ok(rota);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao buscar rotas.");
        }
    }

    [HttpPatch("UPDnome")]
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

    [HttpPatch("RmvFiscais")]
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

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteRotaCommand command)
    {
        try
        {
            await _deleteRota.Handler(command);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao deletar rota.");
        }
    }

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
