using MediatR;
using MongoDB.Driver;
using Pomodorium.TaskManagement.Model.Activities;
using System.DomainModel;

namespace Pomodorium.Features.ActivityManager;

public class MongoDBActivityQueryItemsProjection :
    IRequestHandler<GetActivitiesRequest, GetActivitiesResponse>,
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

    public async Task<GetActivitiesResponse> Handle(GetActivitiesRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<ActivityQueryItem>.Filter.Empty;

        var activityQueryItems = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

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

        await _mongoCollection.InsertOneAsync(activityQueryItem, null, cancellationToken);
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var filter = Builders<ActivityQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var activityQueryItem = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (activityQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        var update = Builders<ActivityQueryItem>.Update
            .Set(x => x.Name, notification.Name)
            .Set(x => x.State, notification.State)
            .Set(x => x.StartDateTime, notification.StartDateTime)
            .Set(x => x.StopDateTime, notification.StopDateTime)
            .Set(x => x.Duration, notification.Duration)
            .Set(x => x.Description, notification.Description)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<ActivityQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var activityQueryItem = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (activityQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
