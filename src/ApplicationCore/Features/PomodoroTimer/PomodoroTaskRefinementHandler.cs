using Pomodorium.Models.PomodoroTechnique;

namespace Pomodorium.Features.PomodoroTimer;

public class PomodoroTaskRefinementHandler : IRequestHandler<PomodoroTaskRefinementRequest, PomodoroTaskRefinementResponse>
{
    private readonly Repository _repository;

    public PomodoroTaskRefinementHandler(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
    }

    public async Task<PomodoroTaskRefinementResponse> Handle(PomodoroTaskRefinementRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id) ?? throw new EntityNotFoundException();

        pomodoro.RefineTask(request.Task);

        await _repository.Save(pomodoro, request.Version);

        var response = new PomodoroTaskRefinementResponse(request.GetCorrelationId());

        return response;
    }
}
