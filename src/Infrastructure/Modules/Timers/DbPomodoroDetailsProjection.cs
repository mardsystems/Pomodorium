using MediatR;
using System.DomainModel;

namespace Pomodorium.Modules.Timers;

public class IndexDBPomodoroDetailsProjection :
    IRequestHandler<GetPomodoroRequest, GetPomodoroResponse>,
    INotificationHandler<PomodoroCreated>,
    INotificationHandler<PomodoroDescriptionChanged>,
    INotificationHandler<PomodoroArchived>
{
    public async Task<GetPomodoroResponse> Handle(GetPomodoroRequest request, CancellationToken cancellationToken)
    {
        var pomodoroDetails = new PomodoroDetails();

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        var response = new GetPomodoroResponse(request.GetCorrelationId()) { PomodoroDetails = pomodoroDetails };

        return response;
    }

    public async Task Handle(PomodoroCreated request, CancellationToken cancellationToken)
    {
        var pomodoroDetails = new PomodoroDetails
        {
            Id = request.Id,
            StartDateTime = request.StartDateTime,
            Description = request.Description,
            Version = request.Version
        };
    }

    public async Task Handle(PomodoroDescriptionChanged notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = new PomodoroDetails();

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoroDetails.Description = notification.Description;
        pomodoroDetails.Version = notification.Version;
    }

    public async Task Handle(PomodoroArchived notification, CancellationToken cancellationToken)
    {
        var pomodoroDetails = new PomodoroDetails();

        if (pomodoroDetails == null)
        {
            throw new EntityNotFoundException();
        }
    }
}
