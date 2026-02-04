using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Features.Commands.Images.Handler;
using APIRelatorios.Application.Features.Querys.EvidenciaRota;
using APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;
using DocumentFormat.OpenXml.Office2010.Excel;
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
    public async Task<IActionResult> BuscarPorId([FromQuery] BuscarEvidenciaPorIDCommands commands)
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
