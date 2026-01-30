using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Features.Commands.Images.Handler;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("EvidenciaRotas")]
public class EvidenciaRotaController : ControllerBase
{
    private readonly CreateImageHandler _createImageHandler;

    private readonly DeleteImageHandler _deleteImageHandler;

    private readonly UpdateDescricaoImageHandler _updateDescricaoImageHandler;

    public EvidenciaRotaController(UpdateDescricaoImageHandler updateDescricaoImageHandler, DeleteImageHandler deleteImageHandler, CreateImageHandler createImageHandler)
    {
        _updateDescricaoImageHandler = updateDescricaoImageHandler;
        _deleteImageHandler = deleteImageHandler;
        _createImageHandler = createImageHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CriarEvicencias(CreateImageCommand command)
    {
        try
        {
            await _createImageHandler.Handler(command);

            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch]
    public async Task<IActionResult> AlterarEvidencias(UpdateDescricaoImageCommands command)
    {
        try
        {
            await _updateDescricaoImageHandler.Handler(command);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeletarEvidencias(DeleteImageCommands command)
    {
        try
        {
            await _deleteImageHandler.Handler(command);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
