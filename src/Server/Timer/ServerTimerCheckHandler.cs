using MediatR;
using Microsoft.AspNetCore.SignalR;
using Pomodorium.Hubs;
using Pomodorium.Modules.Pomos;

namespace Pomodorium.Timer;

public class ServerTimerCheckHandler : IRequestHandler<PostTimerCheckRequest, PostTimerCheckResponse>
{
    private readonly IHubContext<EventHub, IHubEvent> _eventHub;

    public ServerTimerCheckHandler(IHubContext<EventHub, IHubEvent> eventHub)
    {
        _eventHub = eventHub;
    }

    public Task<PostTimerCheckResponse> Handle(PostTimerCheckRequest request, CancellationToken cancellationToken)
    {
        var pomosRunning = new List<Pomodoro>();

        foreach (var item in pomosRunning)
        {

        }

        var response = new PostTimerCheckResponse();

        return Task.FromResult(response);
    }
}
