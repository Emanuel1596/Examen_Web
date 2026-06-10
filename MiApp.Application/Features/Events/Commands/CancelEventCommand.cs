using FluentValidation;
using MediatR;
using MiApp.Application.Features.Events;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands;

public record CancelEventCommand(int Id) : IRequest<EventDto>;

public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, EventDto>
{
    private readonly IEventRepository _eventRepository;

    public CancelEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventDto> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        if (eventEntity is null)
            throw new KeyNotFoundException("Evento no encontrado.");

        eventEntity.Status = EventStatus.Cancelado;

        await _eventRepository.SaveChangesAsync(cancellationToken);

        return eventEntity.ToDto();
    }
}

public class CancelEventCommandValidator : AbstractValidator<CancelEventCommand>
{
    public CancelEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id del evento no es válido.");
    }
}