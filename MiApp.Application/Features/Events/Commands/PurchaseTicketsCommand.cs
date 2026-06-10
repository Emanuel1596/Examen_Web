using FluentValidation;
using MediatR;
using MiApp.Application.Features.Events;
using MiApp.Domain.Entities;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands;

public record PurchaseTicketsCommand(
    int EventId,
    int TicketZoneId,
    int Quantity
) : IRequest<TicketPurchaseDto>;

public class PurchaseTicketsCommandHandler : IRequestHandler<PurchaseTicketsCommand, TicketPurchaseDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly ITicketPurchaseRepository _ticketPurchaseRepository;

    public PurchaseTicketsCommandHandler(
        IEventRepository eventRepository,
        ITicketPurchaseRepository ticketPurchaseRepository)
    {
        _eventRepository = eventRepository;
        _ticketPurchaseRepository = ticketPurchaseRepository;
    }

    public async Task<TicketPurchaseDto> Handle(PurchaseTicketsCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);

        if (eventEntity is null)
            throw new KeyNotFoundException("Evento no encontrado.");

        if (eventEntity.Status != EventStatus.Activo)
            throw new InvalidOperationException("No se pueden comprar boletos de un evento cancelado.");

        var ticketZone = eventEntity.TicketZones.FirstOrDefault(z => z.Id == request.TicketZoneId);

        if (ticketZone is null)
            throw new KeyNotFoundException("Zona de boletaje no encontrada.");

        var total = ticketZone.Price * request.Quantity;

        var purchase = new TicketPurchase
        {
            EventId = eventEntity.Id,
            TicketZoneId = ticketZone.Id,
            Quantity = request.Quantity,
            Total = total,
            PurchaseDate = DateTime.UtcNow,
            Event = eventEntity,
            TicketZone = ticketZone
        };

        await _ticketPurchaseRepository.AddAsync(purchase, cancellationToken);

        return purchase.ToDto();
    }
}

public class PurchaseTicketsCommandValidator : AbstractValidator<PurchaseTicketsCommand>
{
    public PurchaseTicketsCommandValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("El evento es requerido.");

        RuleFor(x => x.TicketZoneId)
            .GreaterThan(0).WithMessage("La zona es requerida.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.")
            .LessThanOrEqualTo(10).WithMessage("Solo se pueden comprar máximo 10 boletos por operación.");
    }
}