using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Features.Commands.User.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{
    private readonly LoginHandler _loginHandler;

    public LoginController(LoginHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }

    [HttpPost]
    public async Task<IActionResult> login(LoginCommands login)
    {
        try
        {
            var jwt = await _loginHandler.Handler(login);

            return Ok(jwt);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
