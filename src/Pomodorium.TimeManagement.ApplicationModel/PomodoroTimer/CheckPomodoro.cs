using Pomodorium.PomodoroTechnique;

namespace Pomodorium.TimeManagement.PomodoroTimer;

public class CheckPomodoroRequest : Request<CheckPomodoroResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}

public class CheckPomodoroResponse : Response
{
    public CheckPomodoroResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public CheckPomodoroResponse()
    {

    }
}

public class CheckPomodoroHandler : IRequestHandler<CheckPomodoroRequest, CheckPomodoroResponse>
{
    private readonly Repository _repository;

    public CheckPomodoroHandler(Repository pomodoroRepository)
    {
        _repository = pomodoroRepository;
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
}
