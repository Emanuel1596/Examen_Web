using MediatR;
using MiApp.Application.Features.Events;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Queries;

public record GetSalesDashboardQuery() : IRequest<SalesDashboardDto>;

public class GetSalesDashboardQueryHandler : IRequestHandler<GetSalesDashboardQuery, SalesDashboardDto>
{
    private readonly ITicketPurchaseRepository _ticketPurchaseRepository;

    public GetSalesDashboardQueryHandler(ITicketPurchaseRepository ticketPurchaseRepository)
    {
        _ticketPurchaseRepository = ticketPurchaseRepository;
    }

    public async Task<SalesDashboardDto> Handle(GetSalesDashboardQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _ticketPurchaseRepository.GetAllAsync(cancellationToken);

        var salesByEvent = purchases
            .GroupBy(p => new { p.EventId, p.Event.Name })
            .Select(g => new SalesByEventDto(
                g.Key.EventId,
                g.Key.Name,
                g.Sum(p => p.Quantity),
                g.Sum(p => p.Total)
            ))
            .OrderByDescending(x => x.TotalSales)
            .ToList();

        return new SalesDashboardDto(
            purchases.Count,
            purchases.Sum(p => p.Quantity),
            purchases.Sum(p => p.Total),
            salesByEvent
        );
    }
}