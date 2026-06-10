using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Infrastructure.Persistence.Repositories;

public class TicketPurchaseRepository : ITicketPurchaseRepository
{
    private readonly AppDbContext _context;

    public TicketPurchaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TicketPurchase>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TicketPurchases
            .Include(p => p.Event)
            .Include(p => p.TicketZone)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TicketPurchase purchase, CancellationToken cancellationToken = default)
    {
        _context.TicketPurchases.Add(purchase);
        await _context.SaveChangesAsync(cancellationToken);
    }
}