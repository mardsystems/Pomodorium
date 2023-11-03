using MediatR;
using MongoDB.Driver;
using System.DomainModel;

namespace Pomodorium.Modules.Pomos;

public class MongoDBPomodoroQueryItemsProjection :
    IRequestHandler<GetPomosRequest, GetPomosResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroDescriptionChanged>,
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
            State = notification.State,
            Description = notification.Description,
            Version = notification.Version
        };

        await _mongoCollection.InsertOneAsync(pomodoroQueryItem, null, cancellationToken);
    }

    public async Task Handle(PomodoroDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var pomodoroQueryItem = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroQueryItem.Description = notification.Description;
        pomodoroQueryItem.Version = notification.Version;

        var update = Builders<PomodoroQueryItem>.Update
            .Set(x => x.Description, notification.Description)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroQueryItem>.Filter.Eq(x => x.Id, notification.Id);

        var pomodoroQueryItem = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
