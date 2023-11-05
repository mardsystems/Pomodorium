using MediatR;
using Pomodorium.Data;
using System.DomainModel;

namespace Pomodorium.Modules.Activities;

public class IndexedDBActivityDetailsProjection :
    IRequestHandler<GetActivityRequest, GetActivityResponse>,
    INotificationHandler<ActivityCreated>,
    INotificationHandler<ActivityUpdated>,
    INotificationHandler<ActivityDeleted>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBActivityDetailsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<GetActivityResponse> Handle(GetActivityRequest request, CancellationToken cancellationToken)
    {
        var activityDetails = await _db.GetAsync<ActivityDetails>("ActivityDetails", request.Id);

        if (activityDetails == null)
        {
            throw new EntityNotFoundException();
        }

        var response = new GetActivityResponse(request.GetCorrelationId()) { ActivityDetails = activityDetails };

        return response;
    }

    public async Task Handle(ActivityCreated notification, CancellationToken cancellationToken)
    {
        var activityDetails = new ActivityDetails
        {
            Id = notification.Id,
            Name = notification.Name,
            State = notification.State,
            StartDateTime = notification.StartDateTime,
            StopDateTime = notification.StopDateTime,
            Duration = notification.Duration,
            Description = notification.Description,
            Version = notification.Version
        };

        await _db.PutAsync("ActivityDetails", activityDetails);
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var activityDetails = await _db.GetAsync<ActivityDetails>("ActivityDetails", notification.Id);

        if (activityDetails == null)
        {
            throw new EntityNotFoundException();
        }

        activityDetails.Name = notification.Name;
        activityDetails.State = notification.State;
        activityDetails.StartDateTime = notification.StartDateTime;
        activityDetails.StopDateTime = notification.StopDateTime;
        activityDetails.Duration = notification.Duration;
        activityDetails.Description = notification.Description;
        activityDetails.Version = notification.Version;

        await _db.PutAsync("ActivityDetails", activityDetails);
    }

    public async Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        var activityDetails = await _db.GetAsync<ActivityDetails>("ActivityDetails", notification.Id);

        if (activityDetails == null)
        {
            throw new EntityNotFoundException();
        }

        await _db.RemoveAsync("ActivityDetails", notification.Id);
    }
}
