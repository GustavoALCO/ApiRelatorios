using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Features.Commands.Rota.Handler;
using APIRelatorios.Application.Features.Commands.User;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("a")]
public class RotaController : ControllerBase
{

    private readonly AddFiscalRotaHandler _addFiscal;

    private readonly CreateRotaHandler _createRota;

    public RotaController(AddFiscalRotaHandler addFiscal, CreateRotaHandler createRota)
    {
        _addFiscal = addFiscal;
        _createRota = createRota;
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
            return BadRequest(ex);
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
            return BadRequest(ex);
        }
    }
}
