using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using System.DomainModel;
using System.DomainModel.Storage;
using System.Reactive.Subjects;

namespace Pomodorium.Hubs;

public class EventHubClient : IDisposable
{
    private readonly HubConnection _server;

    private readonly IAppendOnlyStore _appendOnlyStore;

    private readonly IMediator _mediator;

    public Subject<Event> Notification { get; } = new Subject<Event>();

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

        await DispatchEvent(eventRecord);
    }

    public async Task NotifyOthers(EventAppended notification)
    {
        await _server.SendAsync("NotifyOthers", notification);
    }

    public async Task<GetEventsResponse> GetEvents(GetEventsRequest request)
    {
        return await _server.InvokeAsync<GetEventsResponse>("GetEvents", request);
    }

    public async Task DispatchEvents()
    {
        var request = new GetEventsRequest { };

        var response = await _server.InvokeAsync<GetEventsResponse>("GetEvents", request);

        var eventRecords = response.EventRecords; //.OrderBy(x => x.Date);

        foreach (var eventRecord in eventRecords)
        {
            await DispatchEvent(eventRecord);
        }
    }

    private async Task DispatchEvent(EventRecord eventRecord)
    {
        var _ = Guid.Parse(eventRecord.Name);

        var type = Type.GetType(eventRecord.TypeName) ?? throw new InvalidOperationException();

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

        @event.Version = eventRecord.Version;

        await _mediator.Publish(@event);

        Notification.OnNext(@event);
    }

    private static bool ConflictsWith(Event event1, Event event2)
    {
        return event1.GetType() == event2.GetType();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _server.Remove("Notify");

        Notification.OnCompleted();

        Notification.Dispose();
    }
}
