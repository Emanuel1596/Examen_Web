using MiApp.Domain.Enums;

namespace MiApp.Domain.Entities;

public class Event
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Place { get; set; } = string.Empty;

    public EventStatus Status { get; set; } = EventStatus.Activo;

    public List<TicketZone> TicketZones { get; set; } = new();

    public List<TicketPurchase> Purchases { get; set; } = new();
}