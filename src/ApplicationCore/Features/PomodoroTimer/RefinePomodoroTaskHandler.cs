using Pomodorium.Models.PomodoroTechnique;

namespace Pomodorium.Features.PomodoroTimer;

public class RefinePomodoroTaskHandler : IRequestHandler<RefinePomodoroTaskRequest, RefinePomodoroTaskResponse>
{
    private readonly Repository _repository;

    public RefinePomodoroTaskHandler(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
    }

    public async Task<RefinePomodoroTaskResponse> Handle(RefinePomodoroTaskRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id) ?? throw new EntityNotFoundException();

        pomodoro.RefineTask(request.Task);

        await _repository.Save(pomodoro, request.Version);

        var response = new RefinePomodoroTaskResponse(request.GetCorrelationId());

        return response;
    }
}
