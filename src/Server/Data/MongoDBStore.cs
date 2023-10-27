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

    public EventRecord Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1)
    {
        var version = GetMaxVersion(name, expectedVersion);

        var @event = new EventRecord(name, typeName, version + 1, date, data);

        _mongoCollection.InsertOne(@event);

        return @event;
    }

    public void Append(EventRecord tapeRecord)
    {
        var version = GetMaxVersion(tapeRecord.Name, -1);

        var @event = new EventRecord(tapeRecord.Name, tapeRecord.TypeName, version + 1, tapeRecord.Date, tapeRecord.Data);

        _mongoCollection.InsertOne(@event);
    }

    private long GetMaxVersion(string name, long expectedVersion)
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
            version = _mongoCollection.Aggregate(pipeline).Single();
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

    public IEnumerable<EventRecord> ReadRecords(string name, long afterVersion, long maxCount)
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
            var events = _mongoCollection.Find(filter).Sort(sort).ToList();

            return events;
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, "ReadRecords");

            throw;
        }
    }

    public IEnumerable<EventRecord> ReadRecords(long afterVersion, long maxCount)
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

        var filter = builder.Gt(x => x.Version, afterVersion);

        var sort = Builders<EventRecord>.Sort.Ascending(x => x.Version);

        var events = _mongoCollection.Find(filter).Sort(sort).ToList();

        return events;
    }

    public void Close()
    {

    }

    public void Dispose()
    {

    }
}
