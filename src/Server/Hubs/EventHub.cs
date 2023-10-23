using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.DomainModel;
using System.DomainModel.EventStore;

namespace Pomodorium.Hubs
{
    public class EventHub : Hub
    {
        private readonly IAppendOnlyStore appendOnlyStore;

        private readonly IMediator _mediator;

        public EventHub(IAppendOnlyStore appendOnlyStore, IMediator mediator)
        {
            this.appendOnlyStore = appendOnlyStore;

            _mediator = mediator;
        }

        public async Task Append(EventRecord tapeRecord)
        {
            appendOnlyStore.Append(tapeRecord);

            await Clients.Others.SendAsync("Append", tapeRecord);

            //

            var type = Type.GetType(tapeRecord.TypeName);

            var @event = EventStoreRepository.DesserializeEvent(type, tapeRecord.Data);

            @event.Version = tapeRecord.Version;

            @event.Date = tapeRecord.Date;

            await _mediator.Publish(@event);
        }
    }
}
