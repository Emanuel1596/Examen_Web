using MiApp.Domain.Entities;

namespace MiApp.Application.Features.Events;

public record TicketZoneDto(
    int Id,
    string Zone,
    decimal Price
);

public record EventDto(
    int Id,
    string Name,
    string Description,
    DateTime Date,
    string Place,
    string Status,
    List<TicketZoneDto> TicketZones
);

public record TicketPurchaseDto(
    int Id,
    int EventId,
    string EventName,
    int TicketZoneId,
    string Zone,
    int Quantity,
    decimal UnitPrice,
    decimal Total,
    DateTime PurchaseDate
);

public record SalesByEventDto(
    int EventId,
    string EventName,
    int TicketsSold,
    decimal TotalSales
);

public record SalesDashboardDto(
    int TotalPurchases,
    int TotalTicketsSold,
    decimal TotalSales,
    List<SalesByEventDto> SalesByEvent
);

public static class EventMappings
{
    public static EventDto ToDto(this Event eventEntity)
    {
        return new EventDto(
            eventEntity.Id,
            eventEntity.Name,
            eventEntity.Description,
            eventEntity.Date,
            eventEntity.Place,
            eventEntity.Status.ToString(),
            eventEntity.TicketZones
                .OrderBy(z => z.Zone)
                .Select(z => new TicketZoneDto(z.Id, z.Zone.ToString(), z.Price))
                .ToList()
        );
    }

    public static TicketPurchaseDto ToDto(this TicketPurchase purchase)
    {
        return new TicketPurchaseDto(
            purchase.Id,
            purchase.EventId,
            purchase.Event.Name,
            purchase.TicketZoneId,
            purchase.TicketZone.Zone.ToString(),
            purchase.Quantity,
            purchase.TicketZone.Price,
            purchase.Total,
            purchase.PurchaseDate
        );
    }
}