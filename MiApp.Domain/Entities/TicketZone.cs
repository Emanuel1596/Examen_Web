using MiApp.Domain.Enums;

namespace MiApp.Domain.Entities;

public class TicketZone
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public TicketZoneType Zone { get; set; }

    public decimal Price { get; set; }

    public Event Event { get; set; } = null!;

    public List<TicketPurchase> Purchases { get; set; } = new();
}
