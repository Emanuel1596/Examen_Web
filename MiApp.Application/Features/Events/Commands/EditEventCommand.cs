using FluentValidation;
using MediatR;
using MiApp.Application.Features.Events;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands;

public record EditEventCommand(
    int Id,
    string Name,
    string Description,
    DateTime Date,
    string Place,
    decimal VipPrice,
    decimal PreferentePrice,
    decimal GeneralPrice
) : IRequest<EventDto>;

public class EditEventCommandHandler : IRequestHandler<EditEventCommand, EventDto>
{
    private readonly IEventRepository _eventRepository;

    public EditEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventDto> Handle(EditEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        if (eventEntity is null)
            throw new KeyNotFoundException("Evento no encontrado.");

        eventEntity.Name = request.Name.Trim();
        eventEntity.Description = request.Description.Trim();
        eventEntity.Date = request.Date;
        eventEntity.Place = request.Place.Trim();

        SetZonePrice(eventEntity, TicketZoneType.VIP, request.VipPrice);
        SetZonePrice(eventEntity, TicketZoneType.Preferente, request.PreferentePrice);
        SetZonePrice(eventEntity, TicketZoneType.General, request.GeneralPrice);

        await _eventRepository.SaveChangesAsync(cancellationToken);

        return eventEntity.ToDto();
    }

    private static void SetZonePrice(MiApp.Domain.Entities.Event eventEntity, TicketZoneType zone, decimal price)
    {
        var ticketZone = eventEntity.TicketZones.FirstOrDefault(z => z.Zone == zone);

        if (ticketZone is null)
            throw new InvalidOperationException($"No existe la zona {zone} para este evento.");

        ticketZone.Price = price;
    }
}

public class EditEventCommandValidator : AbstractValidator<EditEventCommand>
{
    public EditEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id del evento no es válido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del evento es requerido.")
            .MaximumLength(150).WithMessage("El nombre no debe superar 150 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es requerida.")
            .MaximumLength(1000).WithMessage("La descripción no debe superar 1000 caracteres.");

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.MinValue).WithMessage("La fecha del evento es requerida.");

        RuleFor(x => x.Place)
            .NotEmpty().WithMessage("El lugar es requerido.")
            .MaximumLength(200).WithMessage("El lugar no debe superar 200 caracteres.");

        RuleFor(x => x.VipPrice)
            .GreaterThan(0).WithMessage("El precio VIP debe ser mayor a 0.");

        RuleFor(x => x.PreferentePrice)
            .GreaterThan(0).WithMessage("El precio Preferente debe ser mayor a 0.");

        RuleFor(x => x.GeneralPrice)
            .GreaterThan(0).WithMessage("El precio General debe ser mayor a 0.");
    }
}