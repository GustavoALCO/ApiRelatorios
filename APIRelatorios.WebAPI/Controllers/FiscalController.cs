using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Features.Commands.User.Handlers;
using APIRelatorios.Application.Features.Querys.User;
using APIRelatorios.Application.Features.Querys.User.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("Fiscais")]
public class FiscalController : ControllerBase
{
    private readonly CreateUserHandler _createHandler;

    private readonly UpdateUsuarioHandler _updateUser;

    private readonly UpdatePasswordHandler _updatePassword;

    private readonly DeleteUsuarioHandler _deletehandler;


    private readonly BuscarTodosUsuariosHandler _buscarTodosUsuariosHandler;

    public FiscalController(CreateUserHandler createHandler, DeleteUsuarioHandler deletehandler, UpdateUsuarioHandler updateUser, BuscarTodosUsuariosHandler buscarTodosUsuariosHandler, UpdatePasswordHandler updatePassword)
    {
        _createHandler = createHandler;
        _deletehandler = deletehandler;
        _updateUser = updateUser;
        _buscarTodosUsuariosHandler = buscarTodosUsuariosHandler;
        _updatePassword = updatePassword;
    }

    [Authorize]
    [HttpGet("TodosFiscais")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var fiscal = await _buscarTodosUsuariosHandler.Handler();

            return Ok(fiscal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //[Authorize]
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

    [Authorize]
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

    [Authorize]
    [HttpPatch("Password")]
    public async Task<IActionResult> PathPassword(UpdatePasswordCommand upduser)
    {
        try
        {
            await _updatePassword.Handler(upduser);
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
            await _deletehandler.Handler(deleteuser);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
