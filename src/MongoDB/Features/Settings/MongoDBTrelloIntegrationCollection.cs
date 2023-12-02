using MongoDB.Driver;
using Pomodorium.Models;

namespace Pomodorium.Features.Settings;

public class MongoDBTrelloIntegrationCollection
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<TrelloIntegration> _mongoCollection;

    public MongoDBTrelloIntegrationCollection(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<TrelloIntegration>("TrelloIntegrationCollection");
    }

    public async Task<IEnumerable<TrelloIntegration>> GetTrelloIntegrationList(TrelloIntegration criteria = default, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TrelloIntegration>.Filter.Empty;

        var TrelloIntegrationList = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

        return TrelloIntegrationList;
    }

    public async Task<TrelloIntegration> CreateTrelloIntegration(TrelloIntegration TrelloIntegration, CancellationToken cancellationToken = default)
    {
        await _mongoCollection.InsertOneAsync(TrelloIntegration, null, cancellationToken);

        return TrelloIntegration;
    }

    public async Task<TrelloIntegration> GetTrelloIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TrelloIntegration>.Filter.Eq(x => x.Id, id);

        var TrelloIntegration = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        return TrelloIntegration;
    }

    public async Task<TrelloIntegration> UpdateTrelloIntegration(TrelloIntegration TrelloIntegration, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TrelloIntegration>.Filter.Eq(x => x.Id, TrelloIntegration.Id);

        var update = Builders<TrelloIntegration>.Update
            .Set(x => x.Name, TrelloIntegration.Name)
            .Set(x => x.Key, TrelloIntegration.Key)
            .Set(x => x.Token, TrelloIntegration.Token);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);

        var TrelloIntegrationUpdated = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        return TrelloIntegrationUpdated;
    }

    public async Task DeleteTrelloIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TrelloIntegration>.Filter.Eq(x => x.Id, id);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
