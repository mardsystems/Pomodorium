using System.DomainModel.Storage;

namespace Pomodorium.Data;

public class MemoryStore : IAppendOnlyStore
{
    private readonly string _connectionString;

    private readonly List<EventRecord> _events;

    public MemoryStore(string connectionString)
    {
        _connectionString = connectionString;

        _events = new List<EventRecord>();
    }

    public EventRecord Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1)
    {
        var version = GetMaxVersion(name, expectedVersion);

        var @event = new EventRecord(name, typeName, version + 1, date, data);

        _events.Add(@event);

        return @event;
    }

    public void Append(EventRecord tapeRecord)
    {
        var version = GetMaxVersion(tapeRecord.Name, -1);

        var @event = new EventRecord(tapeRecord.Name, tapeRecord.TypeName, version + 1, tapeRecord.Date, tapeRecord.Data);

        _events.Add(@event);
    }

    private long GetMaxVersion(string name, long expectedVersion)
    {
        var version = _events
            .Where(x => x.Name == name)
            .Select(x => x.Version)
            .DefaultIfEmpty()
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

        var events = _events
            .Where(x => x.Name == name)
            .Where(x => x.Version > afterVersion)
            .OrderBy(x => x.Version)
            .Take(count);

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

        var events = _events
            .Where(x => x.Version > afterVersion)
            .OrderBy(x => x.Version)
            .Take(count);

        return events;
    }

    public void Close()
    {

    }

    public void Dispose()
    {

    }
}
