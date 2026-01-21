using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("a")]
public class ControllerTeste : ControllerBase
{

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        return Ok("Funcionando API");
    }
}
