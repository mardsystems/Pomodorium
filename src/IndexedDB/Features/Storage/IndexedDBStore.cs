using System.DomainModel.Storage;
using Pomodorium.Data;

namespace Pomodorium.Features.Storage;

public class IndexedDBStore : IAppendOnlyStore
{
    private readonly IndexedDBAccessor _db;

    public IndexedDBStore(IndexedDBAccessor db)
    {
        _db = db;
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

        var events = await _db.GetAllAsync<EventRecord>("EventStore");

        var eventsSorted = events
            .OrderBy(x => x.Name)
                .ThenBy(x => x.Version)
            .Take(count);

        return eventsSorted;
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

        var events = await _db.GetEventsAsync(name);

        var eventsSorted = events
            .Where(x => x.Version >= afterVersion)
            .OrderBy(x => x.Version)
            .Take(count);

        return eventsSorted;
    }

    public async Task<EventRecord> Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = EventStore.IRRELEVANT_VERSION)
    {
        var version = await GetMaxVersion(name, expectedVersion);

        var @event = new EventRecord(name, version + 1, date, typeName, data);

        await _db.PutAsync("EventStore", @event);

        return @event;
    }

    public async Task Append(EventRecord tapeRecord)
    {
        var version = await GetMaxVersion(tapeRecord.Name, -1);

        var @event = new EventRecord(tapeRecord.Name, version + 1, tapeRecord.Date, tapeRecord.TypeName, tapeRecord.Data);

        await _db.PutAsync("EventStore", @event);
    }

    private async Task<long> GetMaxVersion(string name, long expectedVersion)
    {
        var events = await _db.GetEventsAsync(name);

        var version = events
            .Select(x => x.Version)
            .DefaultIfEmpty(-1)
            .Max();

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
        GC.SuppressFinalize(this);
    }
}
