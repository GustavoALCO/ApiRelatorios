using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Features.Querys.EvidenciaRota;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("EvidenciaRotas")]
public class EvidenciaRotaController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    private readonly ILogger<EvidenciaRotaController> _logger;

    public EvidenciaRotaController(ILogger<EvidenciaRotaController> logger, IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("Id")]
    public async Task<IActionResult> BuscarPorId( BuscarEvidenciaPorIDQuery commands)
    {
        try
        {
            var evidencias = await _dispatcher.Query<BuscarEvidenciaPorIDQuery, 
                                                     EvidenciaDTO>(commands);

            return Ok(evidencias);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("TodasEvidencias")]
    public async Task<IActionResult> BuscarTodas([FromQuery] BuscarTodasEvidenciasRotaQuery commands)
    {
        try
        {
            var user = await _dispatcher.Query<BuscarTodasEvidenciasRotaQuery, 
            ICollection<EvidenciaDTO>>(commands);

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
            await _dispatcher.Send<CreateEvidenciaCommand>(command);

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
            await _dispatcher.Send<UpdateEvidenciasCommands>(command);

            return Ok();
        }
        catch (Exception ex)
        {
            
            return BadRequest(ex.Message);
        }
    }
}
