using MediatR;
using Microsoft.AspNetCore.SignalR;
using Pomodorium.Hubs;
using PomodoroTechnique.Models;

namespace Pomodorium.Features.Timer;

public class ServerTimerCheckHandler : IRequestHandler<CheckTimerRequest, CheckTimerResponse>
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly IHubContext<EventHub, IHubEvent> _eventHub;
#pragma warning restore IDE0052 // Remove unread private members

    public ServerTimerCheckHandler(IHubContext<EventHub, IHubEvent> eventHub)
    {
        _eventHub = eventHub;
    }

    public Task<CheckTimerResponse> Handle(CheckTimerRequest request, CancellationToken cancellationToken)
    {
        var pomosRunning = new List<Pomodoro>();

        foreach (var _ in pomosRunning)
        {

        }

        var response = new CheckTimerResponse(request.GetCorrelationId());

        return Task.FromResult(response);
    }
}
