using System.DomainModel.Storage;

namespace Pomodorium.Features.Storage;

public class MemoryStore : IAppendOnlyStore
{
    private readonly List<EventRecord> _events;

    public MemoryStore()
    {
        _events = new List<EventRecord>();
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

        var events = _events
            .OrderBy(x => x.Name)
                .ThenBy(x => x.Version)
            .Take(count);

        return await Task.FromResult(events);
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

        var events = _events
            .Where(x => x.Name == name)
            .Where(x => x.Version > afterVersion)
            .OrderBy(x => x.Version)
            .Take(count);

        return await Task.FromResult(events);
    }

    public async Task<EventRecord> Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1)
    {
        var version = await GetMaxVersion(name, expectedVersion);

        var @event = new EventRecord(name, version + 1, date, typeName, data);

        _events.Add(@event);

        return @event;
    }

    public async Task Append(EventRecord tapeRecord)
    {
        var version = await GetMaxVersion(tapeRecord.Name, -1);

        var @event = new EventRecord(tapeRecord.Name, version + 1, tapeRecord.Date, tapeRecord.TypeName, tapeRecord.Data);

        _events.Add(@event);
    }

    private async Task<long> GetMaxVersion(string name, long expectedVersion)
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

        return await Task.FromResult(version);
    }

    public void Close()
    {

    }

    public void Dispose()
    {

    }
}
