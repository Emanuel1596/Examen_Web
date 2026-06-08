using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;
using MiApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MiApp.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        if (await context.Users.AnyAsync()) return;

        var users = new List<User>
        {
            new()
            {
                Name = "Administrador",
                Email = "admin@miapp.com",
                PasswordHash = passwordHasher.Hash("Admin123!"),
                Role = Role.Admin,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Lector Demo",
                Email = "reader@miapp.com",
                PasswordHash = passwordHasher.Hash("Reader123!"),
                Role = Role.Reader,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();
    }
}
