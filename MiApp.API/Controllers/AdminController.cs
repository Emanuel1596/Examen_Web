using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Domain.Interfaces;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AdminController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetAllAsync();
        var result = users.Select(u => new { u.Id, u.Name, u.Email, Role = u.Role.ToString(), u.CreatedAt });
        return Ok(result);
    }

    [HttpGet("dashboard")]
    public IActionResult GetDashboard()
    {
        return Ok(new { message = "Panel de administración", admin = User.Identity!.Name });
    }
}
