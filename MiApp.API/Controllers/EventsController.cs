using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Events.Commands;
using MiApp.Application.Features.Events.Queries;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ISender _sender;

    public EventsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var result = await _sender.Send(new GetEventsQuery(search, OnlyActive: false));
        return Ok(result);
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive([FromQuery] string? search)
    {
        var result = await _sender.Send(new GetEventsQuery(search, OnlyActive: true));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _sender.Send(new GetEventByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        var result = await _sender.Send(new CreateEventCommand(
            request.Name,
            request.Description,
            request.Date,
            request.Place,
            request.VipPrice,
            request.PreferentePrice,
            request.GeneralPrice
        ));

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [FromBody] EditEventRequest request)
    {
        var result = await _sender.Send(new EditEventCommand(
            id,
            request.Name,
            request.Description,
            request.Date,
            request.Place,
            request.VipPrice,
            request.PreferentePrice,
            request.GeneralPrice
        ));

        return Ok(result);
    }

    [HttpPatch("{id:int}/cancel")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _sender.Send(new CancelEventCommand(id));
        return Ok(result);
    }
}

public record CreateEventRequest(
    string Name,
    string Description,
    DateTime Date,
    string Place,
    decimal VipPrice,
    decimal PreferentePrice,
    decimal GeneralPrice
);

public record EditEventRequest(
    string Name,
    string Description,
    DateTime Date,
    string Place,
    decimal VipPrice,
    decimal PreferentePrice,
    decimal GeneralPrice
);