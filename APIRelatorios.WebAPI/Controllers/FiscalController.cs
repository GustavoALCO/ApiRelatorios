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
            var fiscal = await _dispatcher.Query<BuscarTodosusuariosQuery, ICollection<UsuarioDTO>>(new BuscarTodosusuariosQuery());

            return Ok(fiscal);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post(CreateUsuarioCommand createuser)
    {
            await _dispatcher.Send<CreateUsuarioCommand>(createuser);

            return Ok();
    }

    [Authorize]
    [HttpPatch]
    public async Task<IActionResult> Path(AlterarUsuarioCommand upduser)
    {

            await _dispatcher.Send<AlterarUsuarioCommand>(upduser);
            return Ok();
    }

    [Authorize]
    [HttpPatch("Password")]
    public async Task<IActionResult> PathPassword(UpdatePasswordCommand upduser)
    {

            await _dispatcher.Send<UpdatePasswordCommand>(upduser);
            return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteUsuarioCommand deleteuser)
    {

            await _dispatcher.Send<DeleteUsuarioCommand>(deleteuser);

            return Ok();

    }
}
