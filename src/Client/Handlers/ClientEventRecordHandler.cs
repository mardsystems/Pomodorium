using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Pomodorium.Hubs;
using Pomodorium.Modules.Timers;
using System.DomainModel;
using System.DomainModel.Storage;

namespace Pomodorium.Handlers;

public class ClientEventRecordHandler :
    INotificationHandler<EventRecord>
{
    private readonly EventRecordHubClient _eventRecordHubClient;

    public ClientEventRecordHandler(EventRecordHubClient eventRecordHubClient)
    {
        _eventRecordHubClient = eventRecordHubClient;
    }

    //public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    //{
    //    if (_eventHubClient.Connection.State == HubConnectionState.Disconnected)
    //    {
    //        await _eventHubClient.Connection.StartAsync();
    //    }

    //    if (notification.IsHandled)
    //    {
    //        return;
    //    }

    //    //await _eventHubClient.Connection.SendAsync("OnPomodoroCreated", notification);

    //    var name = notification.Id.ToString();

    //    var expectedVersion = notification.Version - 1;

    //    await _eventHubClient.Connection.SendAsync("Append", name, notification.Date, null, expectedVersion);
    //}

    public async Task Handle(EventRecord notification, CancellationToken cancellationToken)
    {
        if (_eventRecordHubClient.Connection.State == HubConnectionState.Disconnected)
        {
            await _eventRecordHubClient.Connection.StartAsync();
        }

        //if (notification.IsHandled)
        //{
        //    return;
        //}

        //await _eventHubClient.Connection.SendAsync("OnPomodoroCreated", notification);

        //var name = notification.Name;

        //var expectedVersion = notification.Version - 1;

        //var data = EventStoreRepository.SerializeEvent(notification);

        await _eventRecordHubClient.Connection.SendAsync("AppendToOthers", notification);
    }
}
