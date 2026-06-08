using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Auth.Commands.Login;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }
}
