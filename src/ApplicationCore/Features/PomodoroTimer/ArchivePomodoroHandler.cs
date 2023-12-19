using Pomodorium.Models.PomodoroTechnique;

namespace Pomodorium.Features.PomodoroTimer;

public class ArchivePomodoroHandler : IRequestHandler<ArchivePomodoroRequest, ArchivePomodoroResponse>
{
    private readonly Repository _repository;

    public ArchivePomodoroHandler(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
    }

    public async Task<ArchivePomodoroResponse> Handle(ArchivePomodoroRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id) ?? throw new EntityNotFoundException();

        pomodoro.Archive();

        await _repository.Save(pomodoro, request.Version);

        var response = new ArchivePomodoroResponse(request.GetCorrelationId()) { };

        return response;
    }
}
