using MediatR;
using MongoDB.Driver;
using System.DomainModel;

namespace Pomodorium.Modules.Activities;

public class MongoDBActivityDetailsProjection :
    IRequestHandler<GetActivityRequest, GetActivityResponse>,
    INotificationHandler<ActivityCreated>,
    INotificationHandler<ActivityUpdated>,
    INotificationHandler<ActivityDeleted>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<ActivityDetails> _mongoCollection;

    public MongoDBActivityDetailsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<ActivityDetails>("ActivityDetails");
    }

    public async Task<GetActivityResponse> Handle(GetActivityRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<ActivityDetails>.Filter.Eq(x => x.Id, request.Id);

        var activityDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

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

        await _mongoCollection.InsertOneAsync(activityDetails, null, cancellationToken);
    }

    public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
    {
        var filter = Builders<ActivityDetails>.Filter.Eq(x => x.Id, notification.Id);

        var activityDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (activityDetails == null)
        {
            throw new EntityNotFoundException();
        }

        var update = Builders<ActivityDetails>.Update
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
        var filter = Builders<ActivityDetails>.Filter.Eq(x => x.Id, notification.Id);

        var activityDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (activityDetails == null)
        {
            throw new EntityNotFoundException();
        }

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
