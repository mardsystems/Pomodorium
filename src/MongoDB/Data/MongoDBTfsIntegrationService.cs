using MongoDB.Driver;
using Pomodorium.Models;
using Pomodorium.Repositories;

namespace Pomodorium.Data;

public class MongoDBTfsIntegrationService : ITfsIntegrationRepository
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<TfsIntegration> _mongoCollection;

    public MongoDBTfsIntegrationService(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<TfsIntegration>("TfsIntegrationCollection");
    }

    public async Task<IEnumerable<TfsIntegration>> GetTfsIntegrationList(TfsIntegration? criteria = default, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TfsIntegration>.Filter.Empty;

        var tfsIntegrationList = await _mongoCollection.Find(filter).ToListAsync(cancellationToken);

        return tfsIntegrationList;
    }

    public async Task<TfsIntegration> CreateTfsIntegration(TfsIntegration tfsIntegration, CancellationToken cancellationToken = default)
    {
        await _mongoCollection.InsertOneAsync(tfsIntegration, null, cancellationToken);

        return tfsIntegration;
    }

    public async Task<TfsIntegration> GetTfsIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TfsIntegration>.Filter.Eq(x => x.Id, id);

        var tfsIntegration = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        return tfsIntegration;
    }

    public async Task<TfsIntegration> UpdateTfsIntegration(TfsIntegration tfsIntegration, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TfsIntegration>.Filter.Eq(x => x.Id, tfsIntegration.Id);

        var update = Builders<TfsIntegration>.Update
            .Set(x => x.Name, tfsIntegration.Name)
            .Set(x => x.OrganizationName, tfsIntegration.OrganizationName)
            .Set(x => x.PersonalAccessToken, tfsIntegration.PersonalAccessToken)
            .Set(x => x.ProjectName, tfsIntegration.ProjectName);

        await _mongoCollection.UpdateManyAsync(filter, update, null, cancellationToken);

        var tfsIntegrationUpdated = await _mongoCollection.Find(filter).FirstAsync(cancellationToken);

        return tfsIntegrationUpdated;
    }

    public async Task DeleteTfsIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TfsIntegration>.Filter.Eq(x => x.Id, id);

        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}
