using MediatR;
using Pomodorium.Data;
using System.DomainModel;
using TaskManagement.Models.Activities;

namespace Pomodorium.Features.ActivityManager;

public class IndexedDBActivityQueryItemsProjection :
    IRequestHandler<ActivityQueryRequest, ActivityQueryResponse>,
    INotificationHandler<ActivityCreated>,
    INotificationHandler<ActivityUpdated>,
    INotificationHandler<ActivityDeleted>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBActivityQueryItemsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<ActivityQueryResponse> Handle(ActivityQueryRequest request, CancellationToken cancellationToken)
    {
        var activityQueryItems = await _db.GetAllAsync<ActivityQueryItem>("ActivityQueryItems");

        var response = new ActivityQueryResponse(request.GetCorrelationId())
        {
            ActivityQueryItems = activityQueryItems
        };

        return response;
    }

    public async Task Handle(ActivityCreated notification, CancellationToken cancellationToken)
    {
        var activityQueryItem = new ActivityQueryItem
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

        await _db.PutAsync("ActivityQueryItems", activityQueryItem);
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var activityQueryItem = await _db.GetAsync<ActivityQueryItem>("ActivityQueryItems", notification.ActivityId) ?? throw new EntityNotFoundException();

        activityQueryItem.Name = notification.ActivityName;
        activityQueryItem.State = notification.ActivityState;
        activityQueryItem.StartDateTime = notification.StartDateTime;
        activityQueryItem.StopDateTime = notification.StopDateTime;
        activityQueryItem.Duration = notification.ActivityDuration;
        activityQueryItem.Description = notification.ActivityDescription;
        activityQueryItem.Version = notification.Version;

        await _db.PutAsync("ActivityQueryItems", activityQueryItem);
    }

    public async Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        var _ = await _db.GetAsync<ActivityQueryItem>("ActivityQueryItems", notification.ActivityId) ?? throw new EntityNotFoundException();

        await _db.RemoveAsync("ActivityQueryItems", notification.ActivityId);
    }
}
