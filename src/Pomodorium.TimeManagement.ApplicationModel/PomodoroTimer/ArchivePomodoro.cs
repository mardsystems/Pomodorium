using Pomodorium.PomodoroTechnique;

namespace Pomodorium.TimeManagement.PomodoroTimer;

public class ArchivePomodoroRequest : Request<ArchivePomodoroResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}

public class ArchivePomodoroResponse : Response
{
    public ArchivePomodoroResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public ArchivePomodoroResponse()
    {

    }
}

public class ArchivePomodoroHandler : IRequestHandler<ArchivePomodoroRequest, ArchivePomodoroResponse>
{
    private readonly Repository _repository;

    public ArchivePomodoroHandler(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
    }

    public async Task<ArchivePomodoroResponse> Handle(ArchivePomodoroRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id);

        if (pomodoro == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoro.Archive();

        await _repository.Save(pomodoro, request.Version);

        var response = new ArchivePomodoroResponse(request.GetCorrelationId()) { };

        return response;
    }
}
