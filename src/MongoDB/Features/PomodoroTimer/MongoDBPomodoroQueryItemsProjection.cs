using MediatR;
using MongoDB.Driver;
using Pomodorium.PomodoroTechnique.Model;
using System.DomainModel;

namespace Pomodorium.Features.PomodoroTimer;

public class MongoDBPomodoroQueryItemsProjection :
    IRequestHandler<GetPomosRequest, GetPomosResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroChecked>,
    INotificationHandler<PomodoroTaskRefined>,
    INotificationHandler<PomodoroArchived>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<PomodoroQueryItem> _mongoCollection;

    public MongoDBPomodoroQueryItemsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<PomodoroQueryItem>("PomodoroQueryItems");
    }

    public async Task<GetPomosResponse> Handle(GetPomosRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroQueryItem>.Filter.Empty;

        var pomodoroQueryItems = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

        var response = new GetPomosResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

        return response;
    }

    public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem
        {
            Id = notification.Id,
            Task = notification.Task,
            Timer = notification.Timer,
            StartDateTime = notification.StartDateTime,
            State = notification.State,
            Version = notification.Version
        };

        await _mongoCollection.InsertOneAsync(pomodoroQueryItem, null, cancellationToken);
    }

    public async Task Handle(PomodoroChecked notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var update = Builders<PomodoroQueryItem>.Update
            .Set(x => x.State, notification.State)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async Task Handle(PomodoroTaskRefined notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var update = Builders<PomodoroQueryItem>.Update
            .Set(x => x.Task, notification.Task)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
