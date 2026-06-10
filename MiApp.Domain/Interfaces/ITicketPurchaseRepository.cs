using MiApp.Domain.Entities;

namespace MiApp.Domain.Interfaces;

public interface ITicketPurchaseRepository
{
    Task<List<TicketPurchase>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(TicketPurchase purchase, CancellationToken cancellationToken = default);
}