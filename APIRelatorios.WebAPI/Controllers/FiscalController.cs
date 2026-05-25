using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Features.Commands.User.Handlers;
using APIRelatorios.Application.Features.Queries.User;
using APIRelatorios.Application.Features.Querys.User;
using APIRelatorios.Application.Features.Querys.User.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("Fiscais")]
public class FiscalController : ControllerBase
{

    private readonly IDispatcher _dispatcher;

    public FiscalController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [Authorize]
    [HttpGet("TodosFiscais")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var fiscal = await _dispatcher.Query<BuscarTodosusuariosQuery, ICollection<UsuarioDTO>>(new BuscarTodosusuariosQuery());

            return Ok(fiscal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post(CreateUsuarioCommand createuser)
    {
        try
        {
            await _dispatcher.Send<CreateUsuarioCommand>(createuser);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPatch]
    public async Task<IActionResult> Path(AlterarUsuarioCommand upduser)
    {
        try
        {
            await _dispatcher.Send<AlterarUsuarioCommand>(upduser);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPatch("Password")]
    public async Task<IActionResult> PathPassword(UpdatePasswordCommand upduser)
    {
        try
        {
            await _dispatcher.Send<UpdatePasswordCommand>(upduser);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteUsuarioCommand deleteuser)
    {
        try
        {
            await _dispatcher.Send<DeleteUsuarioCommand>(deleteuser);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
