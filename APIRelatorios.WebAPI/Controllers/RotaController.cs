using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Features.Commands.Rota.Handler;
using APIRelatorios.Application.Features.Commands.User;
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

    public RotaController(AddFiscalRotaHandler addFiscal, CreateRotaHandler createRota, DeleteRotaHandler deleteRota, RemoveFiscalRotaHandler rmvFiscalRota, UpdateNomeRotaHandler updateNomeRota, CreateRelatorioHandler createRelatorio)
    {
        _addFiscal = addFiscal;
        _createRota = createRota;
        _deleteRota = deleteRota;
        _rmvFiscalRota = rmvFiscalRota;
        _updateNomeRota = updateNomeRota;
        _createRelatorio = createRelatorio;
    }

    [HttpPatch("UPDnome")]
    public async Task<IActionResult> Updnome(UpdateNomeRotaCommand command)
    {
        try
        {
            await _updateNomeRota.Handler(command);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("AddFiscais")]
    public async Task<IActionResult> AddFiscais(AddFiscalRotaCommand command)
    {
        try
        {
            await _addFiscal.Handler(command);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("RmvFiscais")]
    public async Task<IActionResult> RemoverFiscais(RemoveFiscalRotaCommand command)
    {
        try
        {
            await _rmvFiscalRota.Handler(command);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateRotaCommand command)
    {
        try
        {
            await _createRota.Handler(command);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteRotaCommand command)
    {
        try
        {
            await _deleteRota.Handler(command);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("CriarRelatorio")]
    public async Task<IActionResult> CriarRelatorio(CreateRelatorioWordCommand command)
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
