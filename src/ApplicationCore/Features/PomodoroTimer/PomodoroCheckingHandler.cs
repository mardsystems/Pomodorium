using Pomodorium.Models.PomodoroTechnique;

namespace Pomodorium.Features.PomodoroTimer;

public class PomodoroCheckingHandler : IRequestHandler<PomodoroCheckingRequest, PomodoroCheckingResponse>
{
    private readonly Repository _repository;

    public PomodoroCheckingHandler(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
    }

    public async Task<PomodoroCheckingResponse> Handle(PomodoroCheckingRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id) ?? throw new EntityNotFoundException();

        pomodoro.Check();

        await _repository.Save(pomodoro, request.Version);

        var response = new PomodoroCheckingResponse(request.GetCorrelationId()) { };

        return response;
    }
}
