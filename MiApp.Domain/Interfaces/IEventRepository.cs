using MiApp.Domain.Entities;

namespace MiApp.Domain.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task AddAsync(Event eventEntity, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}