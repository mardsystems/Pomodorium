using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using System.DomainModel;
using System.DomainModel.Storage;

namespace Pomodorium.Hubs;

public class EventHubClient
{
    private readonly HubConnection _server;

    private readonly IAppendOnlyStore _appendOnlyStore;

    private readonly IMediator _mediator;

    public event Action<Event> NewEvent;

    public EventHubClient(HubConnection connection, IAppendOnlyStore appendOnlyStore, IMediator mediator)
    {
        _server = connection;

        _server.On<EventAppended>("Notify", OnNotifyFromServer);

        _appendOnlyStore = appendOnlyStore;

        _mediator = mediator;
    }

    private async Task OnNotifyFromServer(EventAppended notification)
    {
        var eventRecord = notification.Record;

        var id = Guid.Parse(eventRecord.Name);

        var type = Type.GetType(eventRecord.TypeName);

        var @event = EventStore.DesserializeEvent(type, eventRecord.Data);

        try
        {
            await _appendOnlyStore.Append(eventRecord);
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

            await _appendOnlyStore.Append(eventRecord);
        }

        @event.IsRemote = true;

        await _mediator.Publish(@event);

        NewEvent(@event);
    }

    public async Task NotifyOthers(EventAppended notification)
    {
        await _server.SendAsync("NotifyOthers", notification);
    }

    private bool ConflictsWith(Event event1, Event event2)
    {
        return event1.GetType() == event2.GetType();
    }
}
