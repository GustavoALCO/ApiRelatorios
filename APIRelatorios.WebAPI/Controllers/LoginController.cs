using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Features.Commands.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public LoginController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    

    [HttpPost]
    public async Task<IActionResult> login(LoginCommandsCommand login)
    {

        var jwt = await _dispatcher.Send<LoginCommandsCommand, string>(login);

        return Ok(jwt);

    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> verifyToken()
    {
        return Ok();
    }
}
