using MediatR;
using Microsoft.AspNetCore.SignalR;
using Pomodorium.Hubs;
using System.DomainModel.Storage;

namespace Pomodorium.Handlers;

public class ServerStoredEventHandler : INotificationHandler<EventAppended>
{
    private readonly IHubContext<EventHub, IHubEvent> _eventHub;

    public ServerStoredEventHandler(IHubContext<EventHub, IHubEvent> eventHub)
    {
        _eventHub = eventHub;
    }

    public async Task Handle(EventAppended notification, CancellationToken cancellationToken)
    {
        await _eventHub.Clients.All.Notify(notification);
    }
}
