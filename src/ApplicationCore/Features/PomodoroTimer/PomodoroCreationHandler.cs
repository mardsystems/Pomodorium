using Pomodorium.Models.PomodoroTechnique;

namespace Pomodorium.Features.PomodoroTimer;

public class PomodoroCreationHandler : IRequestHandler<PomodoroCreationRequest, PomodoroCreationResponse>
{
    private readonly Repository _repository;

    public PomodoroCreationHandler(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
    }

    public async Task<PomodoroCreationResponse> Handle(PomodoroCreationRequest request, CancellationToken cancellationToken)
    {
        if (request.Task == null)
        {
            throw new InvalidOperationException();
        }

        var pomodoro = new Pomodoro(request.Task, request.Timer, DateTime.Now);

        await _repository.Save(pomodoro, -1);

        var response = new PomodoroCreationResponse(request.GetCorrelationId()) { };

        return response;
    }
}
