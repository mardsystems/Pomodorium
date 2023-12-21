using MediatR;
using Pomodorium.Data;
using Pomodorium.Models.TaskManagement.Activities;
using System.DomainModel;

namespace Pomodorium.Features.ActivityManager;

public class IndexedDBActivityDetailsProjection :
    IRequestHandler<ActivityDetailsRequest, ActivityDetailsResponse>,
    INotificationHandler<ActivityCreated>,
    INotificationHandler<ActivityUpdated>,
    INotificationHandler<ActivityDeleted>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBActivityDetailsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<ActivityDetailsResponse> Handle(ActivityDetailsRequest request, CancellationToken cancellationToken)
    {
        var activityDetails = await _db.GetAsync<ActivityDetails>("ActivityDetails", request.Id) ?? throw new EntityNotFoundException();

        var response = new ActivityDetailsResponse(request.GetCorrelationId())
        {
            ActivityDetails = activityDetails
        };

        return response;
    }

    public async Task Handle(ActivityCreated notification, CancellationToken cancellationToken)
    {
        var activityDetails = new ActivityDetails
        {
            Id = notification.ActivityId,
            Name = notification.ActivityName,
            State = notification.ActivityState,
            StartDateTime = notification.StartDateTime,
            StopDateTime = notification.StopDateTime,
            Duration = notification.ActivityDuration,
            Description = notification.ActivityDescription,
            Version = notification.Version
        };

        await _db.PutAsync("ActivityDetails", activityDetails);
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var activityDetails = await _db.GetAsync<ActivityDetails>("ActivityDetails", notification.ActivityId) ?? throw new EntityNotFoundException();

        activityDetails.Name = notification.ActivityName;
        activityDetails.State = notification.ActivityState;
        activityDetails.StartDateTime = notification.StartDateTime;
        activityDetails.StopDateTime = notification.StopDateTime;
        activityDetails.Duration = notification.ActivityDuration;
        activityDetails.Description = notification.ActivityDescription;
        activityDetails.Version = notification.Version;

        await _db.PutAsync("ActivityDetails", activityDetails);
    }

    public async Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        var _ = await _db.GetAsync<ActivityDetails>("ActivityDetails", notification.ActivityId) ?? throw new EntityNotFoundException();

        await _db.RemoveAsync("ActivityDetails", notification.ActivityId);
    }
}
