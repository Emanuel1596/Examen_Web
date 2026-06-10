using MediatR;
using MiApp.Application.Features.Events;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Queries;

public record GetEventsQuery(
    string? Search,
    bool OnlyActive,
    string? Place,
    string? Status,
    DateTime? DateFrom,
    DateTime? DateTo
) : IRequest<List<EventDto>>;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<EventDto>>
{
    private readonly IEventRepository _eventRepository;

    public GetEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(cancellationToken);

        if (request.OnlyActive)
        {
            events = events.Where(e => e.Status == EventStatus.Activo).ToList();
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            events = events
                .Where(e =>
                    e.Name.ToLower().Contains(search) ||
                    e.Description.ToLower().Contains(search) ||
                    e.Place.ToLower().Contains(search))
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(request.Place))
        {
            var place = request.Place.Trim().ToLower();

            events = events
                .Where(e => e.Place.ToLower().Contains(place))
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(request.Status) &&
            Enum.TryParse<EventStatus>(request.Status, ignoreCase: true, out var status))
        {
            events = events
                .Where(e => e.Status == status)
                .ToList();
        }

        if (request.DateFrom.HasValue)
        {
            events = events
                .Where(e => e.Date.Date >= request.DateFrom.Value.Date)
                .ToList();
        }

        if (request.DateTo.HasValue)
        {
            events = events
                .Where(e => e.Date.Date <= request.DateTo.Value.Date)
                .ToList();
        }

        return events
            .OrderBy(e => e.Date)
            .Select(e => e.ToDto())
            .ToList();
    }
}