namespace MiApp.Domain.Entities;

public class TicketPurchase
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int TicketZoneId { get; set; }

    public int Quantity { get; set; }

    public decimal Total { get; set; }

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    public Event Event { get; set; } = null!;

    public TicketZone TicketZone { get; set; } = null!;
}