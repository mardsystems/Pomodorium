using MediatR;
using Pomodorium.Data;
using Pomodorium.PomodoroTechnique.Model;
using System.DomainModel;

namespace Pomodorium.Features.PomodoroTimer;

public class IndexedDBPomodoroDetailsProjection :
    IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroChecked>,
    INotificationHandler<PomodoroTaskRefined>,
    INotificationHandler<PomodoroArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBPomodoroDetailsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
    {
        var pomodoroDetails = await _db.GetAsync<PomodoroDetails>("PomodoroDetails", request.Id);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        var response = new GetPomodoroResponse(request.GetCorrelationId()) { PomodoroDetails = pomodoroDetails };

        return response;
    }

    public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = new PomodoroDetails
        {
            Id = notification.Id,
            Task = notification.Task,
            Timer = notification.Timer,
            StartDateTime = notification.StartDateTime,
            State = notification.State,
            Version = notification.Version
        };

        await _db.PutAsync("PomodoroDetails", pomodoroDetails);
    }

    public async Task Handle(PomodoroChecked notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = await _db.GetAsync<PomodoroQueryItem>("PomodoroDetails", notification.Id);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroDetails.State = notification.State;
        pomodoroDetails.Version = notification.Version;

        await _db.PutAsync("PomodoroDetails", pomodoroDetails);
    }

    public async Task Handle(PomodoroTaskRefined notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = await _db.GetAsync<PomodoroQueryItem>("PomodoroDetails", notification.Id);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroDetails.Task = notification.Task;
        pomodoroDetails.Version = notification.Version;

        await _db.PutAsync("PomodoroDetails", pomodoroDetails);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = await _db.GetAsync<PomodoroDetails>("PomodoroDetails", notification.Id);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        await _db.RemoveAsync("PomodoroDetails", notification.Id);
    }
}
