using MongoDB.Driver;
using Pomodorium.Modules.Pomodori;
using System.DomainModel.Storage;
using System.Xml.Linq;

namespace Pomodorium.Data;

public class MongoDBStore : IAppendOnlyStore
{
    private readonly MongoClient _mongoClient;

    public MongoDBStore(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public EventRecord Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1)
    {
        var version = GetMaxVersion(name, expectedVersion);

        var @event = new EventRecord(name, typeName, version + 1, date, data);

        var collection = _mongoClient.GetDatabase("Pomodorium").GetCollection<EventRecord>("EventStore");

        collection.InsertOne(@event);

        return @event;
    }

    public void Append(EventRecord tapeRecord)
    {
        var version = GetMaxVersion(tapeRecord.Name, -1);

        var @event = new EventRecord(tapeRecord.Name, tapeRecord.TypeName, version + 1, tapeRecord.Date, tapeRecord.Data);

        var collection = _mongoClient.GetDatabase("Pomodorium").GetCollection<EventRecord>("EventStore");

        collection.InsertOne(@event);
    }

    private long GetMaxVersion(string name, long expectedVersion)
    {
        var collection = _mongoClient.GetDatabase("Pomodorium").GetCollection<EventRecord>("EventStore");

        var builder = Builders<EventRecord>.Filter;

        var filter = builder.Eq(x => x.Name, name);

        var projectionBuilder = Builders<EventRecord>.Projection;

        var projection = projectionBuilder.Include(x => x.Version);

        var pipeline = new EmptyPipelineDefinition<EventRecord>()
            .Match(filter)
            .Group(x => x.Name, g => g.Max(x => x.Version));

        var version = collection.Aggregate(pipeline).FirstOrDefault();

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

        var collection = _mongoClient.GetDatabase("Pomodorium").GetCollection<EventRecord>("EventStore");

        var builder = Builders<EventRecord>.Filter;

        var filter = builder.Eq(x => x.Name, name) & builder.Gt(x => x.Version, afterVersion);

        var sort = Builders<EventRecord>.Sort.Ascending(x => x.Version);

        var events = collection.Find(filter).Sort(sort).ToList();

        return events;
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

        var collection = _mongoClient.GetDatabase("Pomodorium").GetCollection<EventRecord>("EventStore");

        var builder = Builders<EventRecord>.Filter;

        var filter = builder.Gt(x => x.Version, afterVersion);

        var sort = Builders<EventRecord>.Sort.Ascending(x => x.Version);

        var events = collection.Find(filter).Sort(sort).ToList();

        return events;
    }

    public void Close()
    {

    }

    public void Dispose()
    {

    }
}
