using MediatR;
using Pomodorium.Data;
using System.DomainModel;

namespace Pomodorium.Modules.Timers;

public class IndexedDBPomodoroDetailsProjection :
    IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroDescriptionChanged>,
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
            StartDateTime = notification.StartDateTime,
            Description = notification.Description,
            Version = notification.Version
        };

        await _db.PutAsync("PomodoroDetails", pomodoroDetails);
    }

    public async Task Handle(PomodoroDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = await _db.GetAsync<PomodoroDetails>("PomodoroDetails", notification.Id);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroDetails.Description = notification.Description;
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
