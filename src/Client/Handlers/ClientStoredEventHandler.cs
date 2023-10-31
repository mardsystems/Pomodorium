using MediatR;
using Pomodorium.Hubs;
using System.DomainModel.Storage;

namespace Pomodorium.Handlers;

public class ClientStoredEventHandler : INotificationHandler<EventAppended>
{
    private readonly EventHubClient _eventHubClient;

    public ClientStoredEventHandler(EventHubClient eventHubClient)
    {
        _eventHubClient = eventHubClient;
    }

    public async Task Handle(EventAppended notification, CancellationToken cancellationToken)
    {
        await _eventHubClient.NotifyOthers(notification);
    }
}
