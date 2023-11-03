using MediatR;
using MongoDB.Driver;
using System.DomainModel;

namespace Pomodorium.Modules.Pomos;

public class MongoDBPomodoroDetailsProjection :
    IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroDescriptionChanged>,
    INotificationHandler<PomodoroArchived>
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<PomodoroDetails> _mongoCollection;

    public MongoDBPomodoroDetailsProjection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<PomodoroDetails>("PomodoroDetails");
    }

    public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroDetails>.Filter.Eq(x => x.Id, request.Id);

        var pomodoroDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        var response = new GetPomodoroResponse(request.GetCorrelationId()) { PomodoroDetails = pomodoroDetails };

        return response;
    }

    public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = new PomodoroDetails
        {
            Id = notification.Id,
            State = notification.State,
            Description = notification.Description,
            Version = notification.Version
        };

        await _mongoCollection.InsertOneAsync(pomodoroDetails, null, cancellationToken);
    }

    public async Task Handle(PomodoroDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroDetails>.Filter.Eq(x => x.Id, notification.Id);

        var pomodoroDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroDetails.Description = notification.Description;
        pomodoroDetails.Version = notification.Version;

        var update = Builders<PomodoroDetails>.Update
            .Set(x => x.Description, notification.Description)
            .Set(x => x.Version, notification.Version);

        await _mongoCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var filter = Builders<PomodoroDetails>.Filter.Eq(x => x.Id, notification.Id);

        var pomodoroDetails = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
