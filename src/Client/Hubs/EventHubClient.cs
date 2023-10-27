using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using System.DomainModel;
using System.DomainModel.Storage;

namespace Pomodorium.Hubs;

public class EventHubClient
{
    private readonly HubConnection _connection;

    private readonly EventStore _eventStore;

    private readonly IMediator _mediator;

    public event Action<Event> NewEvent;

    public EventHubClient(HubConnection connection, IMediator mediator, EventStore eventStore)
    {
        _connection = connection;

        connection.On<EventRecord>("Append", Append);

        _mediator = mediator;

        _eventStore = eventStore;
    }

    private async Task Append(EventRecord tapeRecord)
    {
        var id = Guid.Parse(tapeRecord.Name);

        var type = Type.GetType(tapeRecord.TypeName);

        var @event = EventStore.DesserializeEvent(type, tapeRecord.Data);

        try
        {
            await _eventStore.AppendToStream(id, -1, @event); //
        }
        catch (EventStoreConcurrencyException ex)
        {
            var failedEvent = @event;

            foreach (var succededEvent in ex.StoreEvents)
            {
                if (ConflictsWith(failedEvent, succededEvent))
                {
                    var message = $"Conflict between ${failedEvent} and {succededEvent}";

                    throw new RealConcurrencyException(ex);
                }
            }

            await _eventStore.AppendToStream(id, ex.StoreVersion, @event);
        }

        //@event.IsHandled = true;

        await _mediator.Publish(@event);
    }

    private bool ConflictsWith(Event event1, Event event2)
    {
        return event1.GetType() == event2.GetType();
    }

    public HubConnection Connection { get => _connection; }
}
