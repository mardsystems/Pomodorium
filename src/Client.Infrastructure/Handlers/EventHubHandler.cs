using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Pomodorium.Hubs;
using Pomodorium.Modules.Pomodori;

namespace Pomodorium.Handlers;

public class EventHubHandler :
    INotificationHandler<PomodoroCreated>
{
    private readonly EventHubClient _eventHubClient;

    public EventHubHandler(EventHubClient eventHubClient)
    {
        _eventHubClient = eventHubClient;
    }

    public async Task Handle(PomodoroCreated notification, CancellationToken cancellationToken)
    {
        if (_eventHubClient.Connection.State == HubConnectionState.Disconnected)
        {
            await _eventHubClient.Connection.StartAsync();
        }

        if (notification.IsHandled)
        {
            return;
        }

        await _eventHubClient.Connection.SendAsync("OnPomodoroCreated", notification);

        //var name = notification.Id.ToString();

        //var expectedVersion = notification.Version - 1;

        //await connection.SendAsync("Append", name, notification.Date, null, expectedVersion);
    }
}
