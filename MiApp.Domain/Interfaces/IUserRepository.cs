using MiApp.Domain.Entities;

namespace MiApp.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
}
