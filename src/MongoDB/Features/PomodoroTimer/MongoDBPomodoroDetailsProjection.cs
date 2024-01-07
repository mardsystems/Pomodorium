using MediatR;
using MongoDB.Driver;
using PomodoroTechnique.Models;
using System.DomainModel;

namespace PomodoroTechnique.Features.PomodoroTimer;

public class MongoDBPomodoroDetailsProjection :
    IRequestHandler<PomodoroDetailsRequest, PomodoroDetailsResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroChecked>,
    INotificationHandler<PomodoroTaskRefined>,
    INotificationHandler<PomodoroArchived>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<PomodoroDetails> _mongoCollection;

    public MongoDBPomodoroDetailsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<PomodoroDetails>("PomodoroDetails");
    }

    public async Task<PomodoroDetailsResponse> Handle(PomodoroDetailsRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroDetails>.Filter.Eq(x => x.Id, request.Id);

        var pomodoroDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken) ?? throw new EntityNotFoundException();

        var response = new PomodoroDetailsResponse(request.GetCorrelationId())
        {
            PomodoroDetails = pomodoroDetails
        };

        return response;
    }

    public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = new PomodoroDetails
        {
            Id = notification.Id,
            Task = notification.Task,
            Timer = notification.Timer,
            StartDateTime = notification.StartDateTime,
            State = notification.State,
            Version = notification.Version
        };

        await _mongoCollection.InsertOneAsync(pomodoroDetails, null, cancellationToken);
    }

    public async Task Handle(PomodoroChecked notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroDetails>.Filter.Eq(x => x.Id, notification.Id);

        var update = Builders<PomodoroDetails>.Update
            .Set(x => x.State, notification.State)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async Task Handle(PomodoroTaskRefined notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroDetails>.Filter.Eq(x => x.Id, notification.Id);

        var update = Builders<PomodoroDetails>.Update
            .Set(x => x.Task, notification.Task)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroDetails>.Filter.Eq(x => x.Id, notification.Id);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
