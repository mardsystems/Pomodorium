using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.DomainModel.Storage;

namespace Pomodorium.Hubs;

public class EventRecordHub : Hub
{
    private readonly IAppendOnlyStore _appendOnlyStore;

    private readonly IMediator _mediator;

    public EventRecordHub(IAppendOnlyStore appendOnlyStore, IMediator mediator)
    {
        _appendOnlyStore = appendOnlyStore;

        _mediator = mediator;
    }
    
    public async Task AppendToAll(EventRecord tapeRecord)
    {
        await Clients.All.SendAsync("Append", tapeRecord);
    }

    public async Task AppendToOthers(EventRecord tapeRecord)
    {
        await _appendOnlyStore.Append(tapeRecord);

        await Clients.Others.SendAsync("Append", tapeRecord);

        //

        var type = Type.GetType(tapeRecord.TypeName);

        var @event = EventStore.DesserializeEvent(type, tapeRecord.Data);

        @event.Version = tapeRecord.Version;

        @event.Date = tapeRecord.Date;

        await _mediator.Publish(@event);
    }
}
