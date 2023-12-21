using MediatR;
using Pomodorium.Data;
using Pomodorium.Models.PomodoroTechnique;
using System.DomainModel;

namespace Pomodorium.Features.PomodoroTimer;


public class IndexedDBPomodoroQueryItemsProjection :
    IRequestHandler<PomodoroQueryRequest, PomodoroQueryResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroChecked>,
    INotificationHandler<PomodoroTaskRefined>,
    INotificationHandler<PomodoroArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBPomodoroQueryItemsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<PomodoroQueryResponse> Handle(PomodoroQueryRequest request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItems = await _db.GetAllAsync<PomodoroQueryItem>("PomodoroQueryItems");

        var response = new PomodoroQueryResponse(request.GetCorrelationId())
        {
            PomodoroQueryItems = pomodoroQueryItems
        };

        return response;
    }

    public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem
        {
            Id = notification.Id,
            Task = notification.Task,
            Timer = notification.Timer,
            StartDateTime = notification.StartDateTime,
            State = notification.State,
            Version = notification.Version
        };

        await _db.PutAsync("PomodoroQueryItems", pomodoroQueryItem);
    }

    public async Task Handle(PomodoroChecked notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = await _db.GetAsync<PomodoroQueryItem>("PomodoroQueryItems", notification.Id) ?? throw new EntityNotFoundException();

        pomodoroQueryItem.State = notification.State;
        pomodoroQueryItem.Version = notification.Version;

        await _db.PutAsync("PomodoroQueryItems", pomodoroQueryItem);
    }

    public async Task Handle(PomodoroTaskRefined notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = await _db.GetAsync<PomodoroQueryItem>("PomodoroQueryItems", notification.Id) ?? throw new EntityNotFoundException();

        pomodoroQueryItem.Task = notification.Task;
        pomodoroQueryItem.Version = notification.Version;

        await _db.PutAsync("PomodoroQueryItems", pomodoroQueryItem);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var _ = await _db.GetAsync<PomodoroQueryItem>("PomodoroQueryItems", notification.Id) ?? throw new EntityNotFoundException();

        await _db.RemoveAsync("PomodoroQueryItems", notification.Id);
    }
}
