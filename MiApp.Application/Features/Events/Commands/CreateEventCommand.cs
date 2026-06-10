using FluentValidation;
using MediatR;
using MiApp.Application.Features.Events;
using MiApp.Domain.Entities;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands;

public record CreateEventCommand(
    string Name,
    string Description,
    DateTime Date,
    string Place,
    decimal VipPrice,
    decimal PreferentePrice,
    decimal GeneralPrice
) : IRequest<EventDto>;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
{
    private readonly IEventRepository _eventRepository;

    public CreateEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = new Event
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Date = request.Date,
            Place = request.Place.Trim(),
            Status = EventStatus.Activo,
            TicketZones =
            [
                new TicketZone { Zone = TicketZoneType.VIP, Price = request.VipPrice },
                new TicketZone { Zone = TicketZoneType.Preferente, Price = request.PreferentePrice },
                new TicketZone { Zone = TicketZoneType.General, Price = request.GeneralPrice }
            ]
        };

        await _eventRepository.AddAsync(eventEntity, cancellationToken);

        return eventEntity.ToDto();
    }
}

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
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