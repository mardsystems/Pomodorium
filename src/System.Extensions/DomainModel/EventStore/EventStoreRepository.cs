using System.Runtime.Serialization.Formatters.Binary;

namespace System.DomainModel.EventStore;

public class EventStoreRepository
{
    private readonly IAppendOnlyStore appendOnlyStore;

    private readonly BinaryFormatter formatter = new BinaryFormatter();

    //private readonly HubConnection connection;

    public event Action<Event> NewEvent;

    public EventStoreRepository(IAppendOnlyStore appendOnlyStore)
    {
        this.appendOnlyStore = appendOnlyStore;

        //this.connection = connection;

        //connection.On<string, DateTime, byte[], long>("Append", Append);

        //connection.StartAsync();
    }

    private void Append(string name, DateTime date, byte[] data, long expectedVersion)
    {
        var @event = DesserializeEvent(data);

        @event.Version = expectedVersion;

        @event.Date = date;

        NewEvent?.Invoke(@event);
    }

    public IEnumerable<Event> LoadAllEvents()
    {
        var records = appendOnlyStore.ReadRecords(0, long.MaxValue);

        var events = new List<Event>();

        foreach (var tapeRecord in records)
        {
            var @event = DesserializeEvent(tapeRecord.Data);

            @event.Version = tapeRecord.Version;

            @event.Date = tapeRecord.Date;

            events.Add(@event);
        }

        return events;
    }

    public IEnumerable<Event> LoadEvents(IIdentity id, long skip, long take)
    {
        var name = IdentityToString(id);

        var records = appendOnlyStore.ReadRecords(name, skip, take).ToList();

        var events = new List<Event>();

        foreach (var tapeRecord in records)
        {
            var @event = DesserializeEvent(tapeRecord.Data);

            @event.Version = tapeRecord.Version;

            @event.Date = tapeRecord.Date;

            events.Add(@event);
        }

        return events;
    }

    private Event DesserializeEvent(byte[] data)
    {
        using (var stream = new MemoryStream(data))
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            return (Event)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        }
    }

    public void AppendToStream(IIdentity id, long originalVersion, IEnumerable<Event> events)
    {
        if (!events.Any())
        {
            return;
        }

        var name = IdentityToString(id);

        var expectedVersion = originalVersion;

        foreach (var @event in events)
        {
            var data = SerializeEvent(@event);

            //connection.SendAsync("Append", name, @event.Date, data, expectedVersion);

            try
            {
                appendOnlyStore.Append(name, @event.Date, data, expectedVersion);

                expectedVersion++;
            }
            catch (AppendOnlyStoreConcurrencyException ex)
            {
                var serverEvents = LoadEvents(id, 0, long.MaxValue);

                var lastEvent = serverEvents.Last();

                throw OptimisticConcurrencyException.Create(lastEvent.Version, ex.ExpectedVersion, id, serverEvents);
            }
        }
    }

    private byte[] SerializeEvent(Event @event)
    {
        using (var stream = new MemoryStream())
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            formatter.Serialize(stream, @event);
#pragma warning restore SYSLIB0011 // Type or member is obsolete

            return stream.ToArray();
        }
    }

    private string IdentityToString(IIdentity id)
    {
        return id.ToString();
    }
}
