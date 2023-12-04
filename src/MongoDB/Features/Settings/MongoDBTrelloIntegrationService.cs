using MongoDB.Driver;
using Pomodorium.Models;
using Pomodorium.Repositories;

namespace Pomodorium.Features.Settings;

public class MongoDBTrelloIntegrationService : ITrelloIntegrationRepository
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<TrelloIntegration> _mongoCollection;

    public MongoDBTrelloIntegrationService(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<TrelloIntegration>("TrelloIntegrationCollection");
    }

    public async Task<IEnumerable<TrelloIntegration>> GetTrelloIntegrationList(TrelloIntegration criteria = default, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TrelloIntegration>.Filter.Empty;

        var trelloIntegrationList = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

        return trelloIntegrationList;
    }

    public async Task<TrelloIntegration> CreateTrelloIntegration(TrelloIntegration trelloIntegration, CancellationToken cancellationToken = default)
    {
        await _mongoCollection.InsertOneAsync(trelloIntegration, null, cancellationToken);

        return trelloIntegration;
    }

    public async Task<TrelloIntegration> GetTrelloIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TrelloIntegration>.Filter.Eq(x => x.Id, id);

        var trelloIntegration = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        return trelloIntegration;
    }

    public async Task<TrelloIntegration> UpdateTrelloIntegration(TrelloIntegration trelloIntegration, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TrelloIntegration>.Filter.Eq(x => x.Id, trelloIntegration.Id);

        var update = Builders<TrelloIntegration>.Update
            .Set(x => x.Name, trelloIntegration.Name)
            .Set(x => x.Key, trelloIntegration.Key)
            .Set(x => x.Token, trelloIntegration.Token);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);

        var trelloIntegrationUpdated = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        return trelloIntegrationUpdated;
    }

    public async Task DeleteTrelloIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TrelloIntegration>.Filter.Eq(x => x.Id, id);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
