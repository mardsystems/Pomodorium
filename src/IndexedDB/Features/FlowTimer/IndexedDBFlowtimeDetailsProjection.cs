using MediatR;
using Pomodorium.Data;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.FlowTimer;

public class IndexedDBFlowtimeDetailsProjection :
    IRequestHandler<FlowtimeDetailsRequest, FlowtimeDetailsResponse>,
    INotificationHandler<FlowtimeCreated>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeArchived>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBFlowtimeDetailsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<FlowtimeDetailsResponse> Handle(FlowtimeDetailsRequest request, CancellationToken cancellationToken)
    {
        var flowtimeDetails = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", request.FlowtimeId) ?? throw new EntityNotFoundException();

        var response = new FlowtimeDetailsResponse(request.GetCorrelationId())
        {
            FlowtimeDetails = flowtimeDetails
        };

        return response;
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeCreated notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = new FlowtimeDetails
        {
            Id = notification.Id,
            CreationDate = notification.CreationDate,
            State = notification.State,
            TaskId = notification.TaskId,
            TaskDescription = notification.TaskDescription,
            TaskVersion = notification.TaskVersion,
            Version = notification.Version
        };

        await _db.PutAsync("FlowtimeDetails", flowtimeDetails);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", notification.FlowtimeId) ?? throw new EntityNotFoundException();

        flowtimeDetails.StartDateTime = notification.StartedAt;
        flowtimeDetails.State = notification.FlowtimeState;
        flowtimeDetails.Version = notification.Version;

        await _db.PutAsync("FlowtimeDetails", flowtimeDetails);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", notification.Id) ?? throw new EntityNotFoundException();

        flowtimeDetails.StopDateTime = notification.StopDateTime;
        flowtimeDetails.Interrupted = notification.Interrupted;
        flowtimeDetails.Worktime = notification.Worktime;
        flowtimeDetails.Breaktime = notification.Breaktime;
        flowtimeDetails.State = notification.State;
        flowtimeDetails.Version = notification.Version;

        await _db.PutAsync("FlowtimeDetails", flowtimeDetails);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var flowtimeDetails = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", notification.Id) ?? throw new EntityNotFoundException();

        flowtimeDetails.StopDateTime = notification.StopDateTime;
        flowtimeDetails.Interrupted = notification.Interrupted;
        flowtimeDetails.Worktime = notification.Worktime;
        flowtimeDetails.Breaktime = notification.Breaktime;
        flowtimeDetails.State = notification.State;
        flowtimeDetails.Version = notification.Version;

        await _db.PutAsync("FlowtimeDetails", flowtimeDetails);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var flowtimeDetailsList = await _db.GetAllAsync<FlowtimeDetails>("FlowtimeDetails");

        var flowtimeDetailsByTaskId = flowtimeDetailsList.Where(x => x.TaskId == notification.TaskId);

        foreach (var flowtimeDetails in flowtimeDetailsByTaskId)
        {
            flowtimeDetails.TaskDescription = notification.TaskDescription;
            flowtimeDetails.TaskVersion = notification.Version;

            await _db.PutAsync("FlowtimeDetails", flowtimeDetails);
        }
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeArchived notification, CancellationToken cancellationToken)
    {
        var _ = await _db.GetAsync<FlowtimeDetails>("FlowtimeDetails", notification.Id) ?? throw new EntityNotFoundException();

        await _db.RemoveAsync("FlowtimeDetails", notification.Id);
    }
}
