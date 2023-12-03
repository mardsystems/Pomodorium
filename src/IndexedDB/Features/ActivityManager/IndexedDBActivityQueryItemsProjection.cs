using MediatR;
using Pomodorium.Data;
using Pomodorium.Models.TaskManagement.Activities;
using System.DomainModel;

namespace Pomodorium.Features.ActivityManager;

public class IndexedDBActivityQueryItemsProjection :
    IRequestHandler<GetActivitiesRequest, GetActivitiesResponse>,
    INotificationHandler<ActivityCreated>,
    INotificationHandler<ActivityUpdated>,
    INotificationHandler<ActivityDeleted>
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBActivityQueryItemsProjection(IndexedDBAccessor db)
    {
        _db = db;
    }

    public async Task<GetActivitiesResponse> Handle(GetActivitiesRequest request, CancellationToken cancellationToken)
    {
        var activityQueryItems = await _db.GetAllAsync<ActivityQueryItem>("ActivityQueryItems");

        var response = new GetActivitiesResponse(request.GetCorrelationId()) { ActivityQueryItems = activityQueryItems };

        return response;
    }

    public async Task Handle(ActivityCreated notification, CancellationToken cancellationToken)
    {
        var activityQueryItem = new ActivityQueryItem
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

        await _db.PutAsync("ActivityQueryItems", activityQueryItem);
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var activityQueryItem = await _db.GetAsync<ActivityQueryItem>("ActivityQueryItems", notification.Id);

        if (activityQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        activityQueryItem.Name = notification.Name;
        activityQueryItem.State = notification.State;
        activityQueryItem.StartDateTime = notification.StartDateTime;
        activityQueryItem.StopDateTime = notification.StopDateTime;
        activityQueryItem.Duration = notification.Duration;
        activityQueryItem.Description = notification.Description;
        activityQueryItem.Version = notification.Version;

        await _db.PutAsync("ActivityQueryItems", activityQueryItem);
    }

    public async Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        var activityQueryItem = await _db.GetAsync<ActivityQueryItem>("ActivityQueryItems", notification.Id);

        if (activityQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        await _db.RemoveAsync("ActivityQueryItems", notification.Id);
    }
}
