using MediatR;
using Pomodorium.Data;
using Pomodorium.PomodoroTechnique;
using System.DomainModel;

namespace Pomodorium.TimeManagement.PomodoroTimer;


public class IndexedDBPomodoroQueryItemsProjection :
    IRequestHandler<GetPomosRequest, GetPomosResponse>,
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

    public async Task<GetPomosResponse> Handle(GetPomosRequest request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItems = await _db.GetAllAsync<PomodoroQueryItem>("PomodoroQueryItems");

        var response = new GetPomosResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

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
        var pomodoroQueryItem = await _db.GetAsync<PomodoroQueryItem>("PomodoroQueryItems", notification.Id);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroQueryItem.State = notification.State;
        pomodoroQueryItem.Version = notification.Version;

        await _db.PutAsync("PomodoroQueryItems", pomodoroQueryItem);
    }

    public async Task Handle(PomodoroTaskRefined notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = await _db.GetAsync<PomodoroQueryItem>("PomodoroQueryItems", notification.Id);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroQueryItem.Task = notification.Task;
        pomodoroQueryItem.Version = notification.Version;

        await _db.PutAsync("PomodoroQueryItems", pomodoroQueryItem);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = await _db.GetAsync<PomodoroQueryItem>("PomodoroQueryItems", notification.Id);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        await _db.RemoveAsync("PomodoroQueryItems", notification.Id);
    }
}
