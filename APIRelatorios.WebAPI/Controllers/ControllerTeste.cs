using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("")]
public class ControllerTeste : ControllerBase
{

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        return Ok("Funcionando API");
    }
}
