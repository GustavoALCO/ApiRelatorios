using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Features.Commands.Images.Handler;
using APIRelatorios.Application.Features.Querys.EvidenciaRota;
using APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("EvidenciaRotas")]
public class EvidenciaRotaController : ControllerBase
{
    private readonly CreateEvidenciaHandler _createImageHandler;

    private readonly DeleteImageHandler _deleteImageHandler;

    private readonly UpdateDescricaoImageHandler _updateDescricaoImageHandler;

    private readonly BuscarTodasAsEvidenciasRotaHandler _buscarEvidencias;

    private readonly BuscarEvidenciaPorIdHandler _buscarId;

    private readonly ILogger<EvidenciaRotaController> _logger;

    public EvidenciaRotaController(UpdateDescricaoImageHandler updateDescricaoImageHandler, DeleteImageHandler deleteImageHandler, CreateEvidenciaHandler createImageHandler, BuscarTodasAsEvidenciasRotaHandler buscarEvidencias, BuscarEvidenciaPorIdHandler buscarId, ILogger<EvidenciaRotaController> logger)
    {
        _updateDescricaoImageHandler = updateDescricaoImageHandler;
        _deleteImageHandler = deleteImageHandler;
        _createImageHandler = createImageHandler;
        _buscarEvidencias = buscarEvidencias;
        _buscarId = buscarId;
        _logger = logger;
    }

    [Authorize]
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

    [Authorize]
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
            _logger.LogError(ex, "Erro ao buscar todas as evidencias da rota com ID {RotaID}", commands.IdRota);
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
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

    [Authorize]
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

    [Authorize]
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
