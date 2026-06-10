using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Events.Commands;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ISender _sender;

    public TicketsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("purchase")]
    [AllowAnonymous]
    public async Task<IActionResult> Purchase([FromBody] PurchaseTicketsRequest request)
    {
        var result = await _sender.Send(new PurchaseTicketsCommand(
            request.EventId,
            request.TicketZoneId,
            request.Quantity
        ));

        return Ok(result);
    }
}

public record PurchaseTicketsRequest(
    int EventId,
    int TicketZoneId,
    int Quantity
);