using Pomodorium.Models.PomodoroTechnique;

namespace Pomodorium.Features.PomodoroTimer;

public class CreatePomodoroHandler : IRequestHandler<CreatePomodoroRequest, CreatePomodoroResponse>
{
    private readonly Repository _repository;

    public CreatePomodoroHandler(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
    }

    public async Task<CreatePomodoroResponse> Handle(CreatePomodoroRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = new Pomodoro(request.Task, request.Timer, DateTime.Now);

        await _repository.Save(pomodoro, -1);

        var response = new CreatePomodoroResponse(request.GetCorrelationId()) { };

        return response;
    }
}
