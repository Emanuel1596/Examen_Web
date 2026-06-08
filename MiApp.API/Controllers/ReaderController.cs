using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Reader")]
public class ReaderController : ControllerBase
{
    [HttpGet("content")]
    public IActionResult GetContent()
    {
        return Ok(new
        {
            message = "Contenido disponible para lectores",
            reader = User.Identity!.Name,
            articles = new[]
            {
                new { id = 1, title = "Artículo 1", summary = "Resumen del artículo 1" },
                new { id = 2, title = "Artículo 2", summary = "Resumen del artículo 2" }
            }
        });
    }

    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        return Ok(new
        {
            name = User.Identity!.Name,
            email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
            role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }
}
