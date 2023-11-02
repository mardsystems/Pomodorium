using MediatR;
using Pomodorium.Data;
using System.DomainModel;

namespace Pomodorium.Modules.Timers;

public class IndexedDBPomodoroQueryItemsProjection :
    IRequestHandler<GetTimersRequest, GetTimersResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroDescriptionChanged>,
    INotificationHandler<PomodoroArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBPomodoroQueryItemsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<GetTimersResponse> Handle(GetTimersRequest request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItems = await _db.GetAllAsync<PomodoroQueryItem>("PomodoroQueryItems");

        var response = new GetTimersResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

        return response;
    }

    public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem
        {
            Id = notification.Id,
            State = notification.State,
            Description = notification.Description,
            Version = notification.Version
        };

        await _db.PutAsync("PomodoroQueryItems", pomodoroQueryItem);
    }

    public async Task Handle(PomodoroDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = await _db.GetAsync<PomodoroQueryItem>("PomodoroQueryItems", notification.Id);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroQueryItem.Description = notification.Description;
        pomodoroQueryItem.Version = notification.Version;

        await _db.PutAsync("PomodoroQueryItems", pomodoroQueryItem);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = await _db.GetAsync<PomodoroDetails>("PomodoroQueryItems", notification.Id);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        await _db.RemoveAsync("PomodoroQueryItems", notification.Id);
    }
}
