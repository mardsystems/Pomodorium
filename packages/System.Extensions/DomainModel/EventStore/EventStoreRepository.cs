using MediatR;
using ProtoBuf;

namespace System.DomainModel.EventStore;

public class EventStoreRepository
{
    private readonly IAppendOnlyStore appendOnlyStore;

    private readonly IMediator _mediator;

    public EventStoreRepository(IAppendOnlyStore appendOnlyStore, IMediator mediator)
    {
        this.appendOnlyStore = appendOnlyStore;

        _mediator = mediator;
    }

    public IEnumerable<Event> LoadAllEvents()
    {
        var records = appendOnlyStore.ReadRecords(0, long.MaxValue);

        var events = new List<Event>();

        foreach (var tapeRecord in records)
        {
            var type = Type.GetType(tapeRecord.TypeName);

            var @event = DesserializeEvent(type, tapeRecord.Data);

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
            var type = Type.GetType(tapeRecord.TypeName);

            var @event = DesserializeEvent(type, tapeRecord.Data);

            @event.Version = tapeRecord.Version;

            @event.Date = tapeRecord.Date;

            events.Add(@event);
        }

        return events;
    }

    public static Event DesserializeEvent(Type type, byte[] data)
    {
        using (var stream = new MemoryStream(data))
        {
            try
            {
                var eventData = Serializer.Deserialize(type, stream);

                var @event = (Event)eventData;

                return @event;
            }
            catch (Exception _)
            {
                throw;
            }
        }
    }

    public async Task AppendToStream(IIdentity id, long originalVersion, params Event[] events)
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

            try
            {
                var eventRecord = appendOnlyStore.Append(name, @event.GetType().AssemblyQualifiedName, @event.Date, data, expectedVersion);

                expectedVersion++;

                await _mediator.Publish(eventRecord);
            }
            catch (AppendOnlyStoreConcurrencyException ex)
            {
                var serverEvents = LoadEvents(id, 0, long.MaxValue);

                var lastEvent = serverEvents.Last();

                throw OptimisticConcurrencyException.Create(lastEvent.Version, ex.ExpectedVersion, id, serverEvents);
            }
        }
    }

    public static byte[] SerializeEvent(Event @event)
    {
        using (var stream = new MemoryStream())
        {
            try
            {
                Serializer.Serialize(stream, @event);
            }
            catch (Exception _)
            {
                throw;
            }

            return stream.ToArray();
        }
    }

    public static string IdentityToString(IIdentity id)
    {
        return id.ToString();
    }
}
