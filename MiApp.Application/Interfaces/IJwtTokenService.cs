using MiApp.Domain.Entities;

namespace MiApp.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
