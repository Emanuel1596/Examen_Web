using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Events.Queries;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DashboardController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("sales")]
    public async Task<IActionResult> GetSales()
    {
        var result = await _sender.Send(new GetSalesDashboardQuery());
        return Ok(result);
    }
}