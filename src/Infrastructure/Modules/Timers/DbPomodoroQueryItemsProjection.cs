using MediatR;
using System.DomainModel;

namespace Pomodorium.Modules.Timers;

public class IndexDBPomodoroQueryItemsProjection :
    IRequestHandler<GetTimersRequest, GetTimersResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroDescriptionChanged>,
    INotificationHandler<PomodoroArchived>
{
    public async Task<GetTimersResponse> Handle(GetTimersRequest request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItems = new PomodoroQueryItem[] { };

        var response = new GetTimersResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

        return await Task.FromResult(response);
    }

    public async Task Handle(PomodoroCreated request, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem
        {
            Id = request.Id.Value,
            StartDateTime = request.StartDateTime,
            Description = request.Description
        };
    }

    public async Task Handle(PomodoroDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem();

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroQueryItem.Description = notification.Description;
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var pomodoroQueryItem = new PomodoroQueryItem();

        if (pomodoroQueryItem == null)
        {
            throw new EntityNotFoundException();
        }
    }
}
