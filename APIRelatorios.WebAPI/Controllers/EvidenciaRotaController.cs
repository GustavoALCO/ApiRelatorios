using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Features.Commands.Images.Handler;
using APIRelatorios.Application.Features.Querys.EvidenciaRota;
using APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("EvidenciaRotas")]
public class EvidenciaRotaController : ControllerBase
{
    private readonly CreateImageHandler _createImageHandler;

    private readonly DeleteImageHandler _deleteImageHandler;

    private readonly UpdateDescricaoImageHandler _updateDescricaoImageHandler;

    private readonly BuscarTodasAsEvidenciasRotaHandler _buscarEvidencias;

    private readonly BuscarEvidenciaPorIdHandler _buscarId;

    public EvidenciaRotaController(UpdateDescricaoImageHandler updateDescricaoImageHandler, DeleteImageHandler deleteImageHandler, CreateImageHandler createImageHandler, BuscarTodasAsEvidenciasRotaHandler buscarEvidencias, BuscarEvidenciaPorIdHandler buscarId)
    {
        _updateDescricaoImageHandler = updateDescricaoImageHandler;
        _deleteImageHandler = deleteImageHandler;
        _createImageHandler = createImageHandler;
        _buscarEvidencias = buscarEvidencias;
        _buscarId = buscarId;
    }

    
    [HttpGet("Id")]
    public async Task<IActionResult> BuscarPorId(Guid commands)
    {
        try
        {
            var evidencias = await _buscarId.Handler(commands);

            return Ok(evidencias);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
    [HttpGet("TodasEvidencias")]
    public async Task<IActionResult> BuscarTodas([FromQuery] BuscarTodasEvidenciasRotaCommands commands)
    {
        try
        {
            var user = await _buscarEvidencias.Handler(commands);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //[authorize]
    [HttpPost]
    public async Task<IActionResult> CriarEvicencias(CreateEvidenciaCommand command)
    {
        try
        {
            await _createImageHandler.Handler(command);

            return Ok();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    //[authorize]
    [HttpPatch]
    public async Task<IActionResult> AlterarEvidencias(UpdateEvidenciasCommands command)
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

    //[authorize]
    [HttpDelete]
    public async Task<IActionResult> DeletarEvidencias(Guid command)
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
