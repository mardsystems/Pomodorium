using Pomodorium.Models.PomodoroTechnique;

namespace Pomodorium.Features.PomodoroTimer;

public class PomodoroArchivingHandler : IRequestHandler<PomodoroArchivingRequest, PomodoroArchivingResponse>
{
    private readonly Repository _repository;

    public PomodoroArchivingHandler(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
    }

    public async Task<PomodoroArchivingResponse> Handle(PomodoroArchivingRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id) ?? throw new EntityNotFoundException();

        pomodoro.Archive();

        await _repository.Save(pomodoro, request.Version);

        var response = new PomodoroArchivingResponse(request.GetCorrelationId()) { };

        return response;
    }
}
