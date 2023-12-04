using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.DomainModel.Storage;

namespace Pomodorium.Hubs;

//[Authorize]
public class EventHub : Hub<IHubEvent>
{
    private readonly IAppendOnlyStore _appendOnlyStore;

    private readonly IMediator _mediator;

    public EventHub(IAppendOnlyStore appendOnlyStore, IMediator mediator)
    {
        _appendOnlyStore = appendOnlyStore;

        _mediator = mediator;
    }

    public async Task NotifyAll(EventAppended notification)
    {
        if (Clients == null)
        {
            return;
        }

        await Clients.All.Notify(notification);
    }

    public async Task NotifyOthers(EventAppended notification)
    {
        await Clients.Others.Notify(notification);

        //

        var eventRecord = notification.Record;

        await _appendOnlyStore.Append(eventRecord);

        //

        var type = Type.GetType(eventRecord.TypeName);

        var @event = EventStore.DesserializeEvent(type, eventRecord.Data);

        @event.Version = eventRecord.Version;

        @event.Date = eventRecord.Date;

        @event.IsRemote = true;

        await _mediator.Publish(@event);
    }

    public async Task<GetEventsResponse> GetEvents(GetEventsRequest request)
    {
        var response = await _mediator.Send<GetEventsResponse>(request);

        return response;
    }
}
