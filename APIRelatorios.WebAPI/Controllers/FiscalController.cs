using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Features.Commands.User.Handlers;
using APIRelatorios.Application.Features.Querys.User;
using APIRelatorios.Application.Features.Querys.User.Handler;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("Fiscais")]
public class FiscalController : ControllerBase
{
    private readonly CreateUserHandler _createHandler;

    private readonly UpdateUsuarioHandler _updateUser;

    private readonly DeleteUsuarioHandler _deletehandler;

    private readonly BuscarUsuarioIdHandler _buscarUsuarioIdHandler;

    public FiscalController(CreateUserHandler createHandler, DeleteUsuarioHandler deletehandler, UpdateUsuarioHandler updateUser, BuscarUsuarioIdHandler buscarUsuarioIdHandler)
    {
        _createHandler = createHandler;
        _deletehandler = deletehandler;
        _updateUser = updateUser;
        _buscarUsuarioIdHandler = buscarUsuarioIdHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] BuscarUsuarioIdCommands command)
    {
        try
        {
            var fiscal = await _buscarUsuarioIdHandler.Handler(command);

            return Ok(fiscal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateUsuarioCommand createuser)
    {
        try
        {
            await _createHandler.Handler(createuser);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch]
    public async Task<IActionResult> Path(AlterarUsuarioCommand upduser)
    {
        try
        {
            await _updateUser.Handler(upduser);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteUsuarioCommand deleteuser)
    {
        try
        {
            await _deletehandler.Handler(deleteuser);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
