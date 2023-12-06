using MediatR;
using Microsoft.AspNetCore.SignalR;
using Pomodorium.Hubs;
using Pomodorium.Models.PomodoroTechnique;

namespace Pomodorium.Features.Timer;

public class ServerTimerCheckHandler : IRequestHandler<CheckTimerRequest, CheckTimerResponse>
{
    private readonly IHubContext<EventHub, IHubEvent> _eventHub;

    public ServerTimerCheckHandler(IHubContext<EventHub, IHubEvent> eventHub)
    {
        _eventHub = eventHub;
    }

    public Task<CheckTimerResponse> Handle(CheckTimerRequest request, CancellationToken cancellationToken)
    {
        var pomosRunning = new List<Pomodoro>();

        foreach (var item in pomosRunning)
        {

        }

        var response = new CheckTimerResponse();

        return Task.FromResult(response);
    }
}
