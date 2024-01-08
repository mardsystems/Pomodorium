using MediatR;
using MongoDB.Driver;
using TaskManagement.Models.Activities;

namespace TaskManagement.Features.ActivityManager;

public class MongoDBActivityQueryItemsProjection :
    IRequestHandler<ActivityQueryRequest, ActivityQueryResponse>,
    INotificationHandler<ActivityCreated>,
    INotificationHandler<ActivityUpdated>,
    INotificationHandler<ActivityDeleted>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<ActivityQueryItem> _mongoCollection;

    public MongoDBActivityQueryItemsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<ActivityQueryItem>("ActivityQueryItems");
    }

    public async Task<ActivityQueryResponse> Handle(ActivityQueryRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<ActivityQueryItem>.Filter.Empty;

        var activityQueryItems = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

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

        await _mongoCollection.InsertOneAsync(activityQueryItem, null, cancellationToken);
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var filter = Builders<ActivityQueryItem>.Filter.Eq(x => x.Id, notification.ActivityId);

        var update = Builders<ActivityQueryItem>.Update
            .Set(x => x.Name, notification.ActivityName)
            .Set(x => x.State, notification.ActivityState)
            .Set(x => x.StartDateTime, notification.StartDateTime)
            .Set(x => x.StopDateTime, notification.StopDateTime)
            .Set(x => x.Duration, notification.ActivityDuration)
            .Set(x => x.Description, notification.ActivityDescription)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<ActivityQueryItem>.Filter.Eq(x => x.Id, notification.ActivityId);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
