using MediatR;
using Pomodorium.Data;
using PomodoroTechnique.Models;
using System.DomainModel;

namespace PomodoroTechnique.Features.PomodoroTimer;

public class IndexedDBPomodoroDetailsProjection :
    IRequestHandler<PomodoroDetailsRequest, PomodoroDetailsResponse>,
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

    public async Task<PomodoroDetailsResponse> Handle(PomodoroDetailsRequest request, CancellationToken cancellationToken)
    {
        var pomodoroDetails = await _db.GetAsync<PomodoroDetails>("PomodoroDetails", request.Id) ?? throw new EntityNotFoundException();

        var response = new PomodoroDetailsResponse(request.GetCorrelationId()) { PomodoroDetails = pomodoroDetails };

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
        var pomodoroDetails = await _db.GetAsync<PomodoroQueryItem>("PomodoroDetails", notification.Id) ?? throw new EntityNotFoundException();

        pomodoroDetails.State = notification.State;
        pomodoroDetails.Version = notification.Version;

        await _db.PutAsync("PomodoroDetails", pomodoroDetails);
    }

    public async Task Handle(PomodoroTaskRefined notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = await _db.GetAsync<PomodoroQueryItem>("PomodoroDetails", notification.Id) ?? throw new EntityNotFoundException();

        pomodoroDetails.Task = notification.Task;
        pomodoroDetails.Version = notification.Version;

        await _db.PutAsync("PomodoroDetails", pomodoroDetails);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var _ = await _db.GetAsync<PomodoroDetails>("PomodoroDetails", notification.Id) ?? throw new EntityNotFoundException();

        await _db.RemoveAsync("PomodoroDetails", notification.Id);
    }
}
