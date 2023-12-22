using MediatR;
using MongoDB.Driver;
using Pomodorium.Models.FlowtimeTechnique;
using Pomodorium.Models.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.Features.FlowTimer;

public class MongoDBFlowtimeDetailsProjection :
    IRequestHandler<FlowtimeDetailsRequest, FlowtimeDetailsResponse>,
    INotificationHandler<FlowtimeCreated>,
    INotificationHandler<FlowtimeStarted>,
    INotificationHandler<FlowtimeInterrupted>,
    INotificationHandler<FlowtimeStopped>,
    INotificationHandler<TaskDescriptionChanged>,
    INotificationHandler<FlowtimeArchived>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<FlowtimeDetails> _mongoCollection;

    public MongoDBFlowtimeDetailsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<FlowtimeDetails>("FlowtimeDetails");
    }

    public async Task<FlowtimeDetailsResponse> Handle(FlowtimeDetailsRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, request.FlowtimeId);

        var flowtimeDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken) ?? throw new EntityNotFoundException();

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

        await _mongoCollection.InsertOneAsync(flowtimeDetails, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStarted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, notification.FlowtimeId);

        var flowtimeDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken) ?? throw new EntityNotFoundException();

        var update = Builders<FlowtimeDetails>.Update
            .Set(x => x.StartDateTime, notification.StartedAt)
            .Set(x => x.State, notification.FlowtimeState)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, notification.Id);

        var update = Builders<FlowtimeDetails>.Update
            .Set(x => x.StopDateTime, notification.StopDateTime)
            .Set(x => x.Interrupted, notification.Interrupted)
            .Set(x => x.Worktime, notification.Worktime)
            .Set(x => x.Breaktime, notification.Breaktime)
            .Set(x => x.State, notification.State)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, notification.Id);

        var flowtimeDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken) ?? throw new EntityNotFoundException();

        flowtimeDetails.StopDateTime = notification.StopDateTime;
        flowtimeDetails.Interrupted = notification.Interrupted;
        flowtimeDetails.Worktime = notification.Worktime;
        flowtimeDetails.Breaktime = notification.Breaktime;
        flowtimeDetails.State = notification.State;
        flowtimeDetails.Version = notification.Version;

        var update = Builders<FlowtimeDetails>.Update
            .Set(x => x.StopDateTime, notification.StopDateTime)
            .Set(x => x.Interrupted, notification.Interrupted)
            .Set(x => x.Worktime, notification.Worktime)
            .Set(x => x.Breaktime, notification.Breaktime)
            .Set(x => x.State, notification.State)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(TaskDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.TaskId, notification.TaskId);

        var update = Builders<FlowtimeDetails>.Update
            .Set(x => x.TaskDescription, notification.TaskDescription)
            .Set(x => x.TaskVersion, notification.Version);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, notification.Id);

        var _ = await _mongoCollection.Find(filter).FirstAsync(cancellationToken) ?? throw new EntityNotFoundException();

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
