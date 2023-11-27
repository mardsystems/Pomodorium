using MediatR;
using MongoDB.Driver;
using Pomodorium.FlowtimeTechnique;
using Pomodorium.TaskManagement.Tasks;
using System.DomainModel;

namespace Pomodorium.TimeManagement.FlowTimer;

public class MongoDBFlowtimeDetailsProjection :
    IRequestHandler<GetFlowtimeRequest, GetFlowtimeResponse>,
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

    public async Task<GetFlowtimeResponse> Handle(GetFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, request.Id);

        var pomodoroDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        var response = new GetFlowtimeResponse(request.GetCorrelationId()) { FlowtimeDetails = pomodoroDetails };

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
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, notification.Id);

        var flowtimeDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        flowtimeDetails.StartDateTime = notification.StartDateTime;
        flowtimeDetails.State = notification.State;
        flowtimeDetails.Version = notification.Version;

        var update = Builders<FlowtimeDetails>.Update
            .Set(x => x.StartDateTime, notification.StartDateTime)
            .Set(x => x.State, notification.State)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeInterrupted notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, notification.Id);

        var flowtimeDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

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

    public async System.Threading.Tasks.Task Handle(FlowtimeStopped notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, notification.Id);

        var flowtimeDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

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
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.TaskId, notification.Id);

        var update = Builders<FlowtimeDetails>.Update
            .Set(x => x.TaskDescription, notification.Description)
            .Set(x => x.TaskVersion, notification.Version);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);
    }

    public async System.Threading.Tasks.Task Handle(FlowtimeArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<FlowtimeDetails>.Filter.Eq(x => x.Id, notification.Id);

        var flowtimeDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (flowtimeDetails == null)
        {
            throw new EntityNotFoundException();
        }

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
