using Pomodorium.PomodoroTechnique;

namespace Pomodorium.TimeManagement.PomodoroTimer;

public class RefinePomodoroTaskRequest : Request<RefinePomodoroTaskResponse>
{
    public Guid Id { get; set; }

    public string Task { get; set; }

    public long Version { get; set; }
}

public class RefinePomodoroTaskResponse : Response
{
    public RefinePomodoroTaskResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public RefinePomodoroTaskResponse()
    {

    }
}

public class RefinePomodoroTaskHandler : IRequestHandler<RefinePomodoroTaskRequest, RefinePomodoroTaskResponse>
{
    private readonly Repository _repository;

    public RefinePomodoroTaskHandler(Repository pomodoroRepository)
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

    public async Task<RefinePomodoroTaskResponse> Handle(RefinePomodoroTaskRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id);

        if (pomodoro == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoro.RefineTask(request.Task);

        await _repository.Save(pomodoro, request.Version);

        var response = new RefinePomodoroTaskResponse(request.GetCorrelationId()) { };

        return response;
    }

    public async Task<CheckPomodoroResponse> Handle(CheckPomodoroRequest request, CancellationToken cancellationToken)
    {
        var pomodoro = await _repository.GetAggregateById<Pomodoro>(request.Id);

        if (pomodoro == null)
        {
            throw new EntityNotFoundException();
        }

        pomodoro.Check();

        await _repository.Save(pomodoro, request.Version);

        var response = new CheckPomodoroResponse(request.GetCorrelationId()) { };

        return response;
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
