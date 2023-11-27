using MongoDB.Driver;
using System.DomainModel.Storage;

namespace Pomodorium.Data;

public class MongoDBStore : IAppendOnlyStore
{
    private readonly MongoClient _mongoClient;

    private readonly IMongoCollection<EventRecord> _mongoCollection;

    private readonly ILogger<MongoDBStore> _logger;

    public MongoDBStore(MongoClient mongoClient, ILogger<MongoDBStore> logger)
    {
        _mongoClient = mongoClient;

        _mongoCollection = _mongoClient.GetDatabase("Pomodorium").GetCollection<EventRecord>("EventStore");

        _logger = logger;
    }

    public async Task<IEnumerable<EventRecord>> ReadRecords(long maxCount)
    {
        int count;

        if (maxCount > int.MaxValue)
        {
            count = int.MaxValue;
        }
        else
        {
            count = (int)maxCount;
        }

        var builder = Builders<EventRecord>.Filter;

        var filter = builder.Empty;

        //var sort = Builders<EventRecord>.Sort
        //    .Ascending(x => x.Name)
        //    .Ascending(x => x.Version);

        var events = await _mongoCollection.Find(filter).ToListAsync();

        return events;
    }

    public async Task<IEnumerable<EventRecord>> ReadRecords(string name, long afterVersion, long maxCount)
    {
        int count;

        if (maxCount > int.MaxValue)
        {
            count = int.MaxValue;
        }
        else
        {
            count = (int)maxCount;
        }

        var builder = Builders<EventRecord>.Filter;

        var filter = builder.Eq(x => x.Name, name) & builder.Gte(x => x.Version, afterVersion);

        var sort = Builders<EventRecord>.Sort.Ascending(x => x.Version);

        try
        {
            var events = await _mongoCollection.Find(filter).Sort(sort).ToListAsync();

            return events;
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, "ReadRecords");

            throw;
        }
    }

    public async Task<EventRecord> Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1)
    {
        var version = await GetMaxVersion(name, expectedVersion);

        var @event = new EventRecord(name, version + 1, date, typeName, data);

        await _mongoCollection.InsertOneAsync(@event);

        return @event;
    }

    public async Task Append(EventRecord tapeRecord)
    {
        var version = await GetMaxVersion(tapeRecord.Name, -1);

        var @event = new EventRecord(tapeRecord.Name, version + 1, tapeRecord.Date, tapeRecord.TypeName, tapeRecord.Data);

        await _mongoCollection.InsertOneAsync(@event);
    }

    private async Task<long> GetMaxVersion(string name, long expectedVersion)
    {
        var builder = Builders<EventRecord>.Filter;

        var filter = builder.Eq(x => x.Name, name);

        var projectionBuilder = Builders<EventRecord>.Projection;

        var projection = projectionBuilder.Include(x => x.Version);

        var pipeline = new EmptyPipelineDefinition<EventRecord>()
            .Match(filter)
            .Group(x => x.Name, g => g.Max(x => x.Version));

        long version;

        try
        {
            version = await _mongoCollection.Aggregate(pipeline).SingleAsync();
        }
        catch (Exception)
        {
            version = -1;
        }

        //var version = _events
        //    .Where(x => x.Name == name)
        //    .Select(x => x.Version)
        //    .DefaultIfEmpty()
        //    .Max();

        if (expectedVersion != -1)
        {
            if (version != expectedVersion)
            {
                throw new AppendOnlyStoreConcurrencyException(version, expectedVersion, name);
            }
        }

        return version;
    }

    public void Close()
    {

    }

    public void Dispose()
    {

    }
}
