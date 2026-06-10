using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Infrastructure.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .Include(e => e.TicketZones)
            .Include(e => e.Purchases)
            .ToListAsync(cancellationToken);
    }

    public async Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .Include(e => e.TicketZones)
            .Include(e => e.Purchases)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task AddAsync(Event eventEntity, CancellationToken cancellationToken = default)
    {
        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}