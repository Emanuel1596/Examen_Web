using MediatR;

namespace MiApp.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

public record LoginResponse(string Token, string Email, string Name, string Role);
