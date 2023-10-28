using MediatR;
using Pomodorium.Hubs;
using System.DomainModel.Storage;

namespace Pomodorium.Handlers;

public class ServerEventRecordHandler :
    INotificationHandler<EventRecord>
{
    private readonly EventRecordHub _eventRecordHub;

    public ServerEventRecordHandler(EventRecordHub eventRecordHub)
    {
        _eventRecordHub = eventRecordHub;
    }

    public async Task Handle(EventRecord notification, CancellationToken cancellationToken)
    {
        await _eventRecordHub.AppendToAll(notification);
    }
}
