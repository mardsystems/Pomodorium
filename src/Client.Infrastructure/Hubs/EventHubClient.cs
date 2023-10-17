using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Pomodorium.Modules.Pomodori;
using System.DomainModel;
using System.DomainModel.EventStore;

namespace Pomodorium.Hubs;

public class EventHubClient
{
    private readonly HubConnection _connection;

    private readonly EventStoreRepository _eventStore;

    private readonly IMediator _mediator;

    public event Action<Event> NewEvent;

    public EventHubClient(HubConnection connection, IMediator mediator, EventStoreRepository eventStore)
    {
        _connection = connection;

        connection.On<PomodoroCreated>("OnPomodoroCreated", OnPomodoroCreated);

        connection.On<string, DateTime, byte[], long>("Append", Append);

        _mediator = mediator;

        _eventStore = eventStore;
    }

    private async Task OnPomodoroCreated(PomodoroCreated @event)
    {
        try
        {
            //_eventStore.AppendToStream(@event.Id, @event.Version, @event);
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

            //_eventStore.AppendToStream(@event.Id, ex.StoreVersion, @event);
        }

        @event.IsHandled = true;

        await _mediator.Publish(@event);
    }

    private bool ConflictsWith(Event event1, Event event2)
    {
        return event1.GetType() == event2.GetType();
    }

    private void Append(string name, DateTime date, byte[] data, long expectedVersion)
    {
        //var @event = DesserializeEvent(data);

        //@event.Version = expectedVersion;

        //@event.Date = date;

        //NewEvent?.Invoke(@event);
    }

    public HubConnection Connection { get => _connection; }
}
