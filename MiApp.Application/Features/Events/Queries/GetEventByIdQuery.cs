using FluentValidation;
using MediatR;
using MiApp.Application.Features.Events;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Queries;

public record GetEventByIdQuery(int Id) : IRequest<EventDto>;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
{
    private readonly IEventRepository _eventRepository;

    public GetEventByIdQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        if (eventEntity is null)
            throw new KeyNotFoundException("Evento no encontrado.");

        return eventEntity.ToDto();
    }
}

public class GetEventByIdQueryValidator : AbstractValidator<GetEventByIdQuery>
{
    public GetEventByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id del evento no es válido.");
    }
}