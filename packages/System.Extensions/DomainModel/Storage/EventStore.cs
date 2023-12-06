using MediatR;
using ProtoBuf;

namespace System.DomainModel.Storage;

public class EventStore
{
    private readonly IAppendOnlyStore _appendOnlyStore;

    private readonly IMediator _mediator;

    public EventStore(IAppendOnlyStore appendOnlyStore, IMediator mediator)
    {
        _appendOnlyStore = appendOnlyStore;

        _mediator = mediator;
    }

    public async Task<IEnumerable<Event>> LoadAllEvents()
    {
        var records = await _appendOnlyStore.ReadRecords(long.MaxValue);

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

    public async Task<IEnumerable<Event>> GetEventsForAggregate(Guid id, long skip, long take)
    {
        var name = id.ToString();

        var records = await _appendOnlyStore.ReadRecords(name, skip, take);

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

    public async Task AppendToStream(Guid id, long originalVersion, params Event[] events)
    {
        if (!events.Any())
        {
            return;
        }

        var name = id.ToString();

        //var version = originalVersion;

        foreach (var @event in events)
        {
            //version++;

            //@event.Version = version;

            var data = SerializeEvent(@event);

            try
            {
                var eventRecord = await _appendOnlyStore.Append(name, @event.GetType().AssemblyQualifiedName, @event.Date, data, originalVersion);

                var eventStored = new EventAppended { Record = eventRecord };

                @event.Version = eventRecord.Version;

                await _mediator.Publish(eventStored);
            }
            catch (AppendOnlyStoreConcurrencyException ex)
            {
                var serverEvents = await GetEventsForAggregate(id, 0, long.MaxValue);

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
}
