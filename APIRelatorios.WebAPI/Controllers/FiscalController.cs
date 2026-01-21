using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Features.Commands.User.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("Fiscais")]
public class FiscalController : ControllerBase
{
    private readonly CreateUserHandler _createHandler;

    private readonly UpdateUsuarioHandler _updateUser;

    private readonly DeleteUsuarioHandler _deletehandler;

    public FiscalController(CreateUserHandler createHandler, DeleteUsuarioHandler deletehandler, UpdateUsuarioHandler updateUser)
    {
        _createHandler = createHandler;
        _deletehandler = deletehandler;
        _updateUser = updateUser;
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
