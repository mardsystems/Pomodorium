using Pomodorium.PomodoroTechnique;

namespace Pomodorium.TimeManagement.PomodoroTimer;

public class CreatePomodoroRequest : Request<CreatePomodoroResponse>
{
    public string? Task { get; set; }

    public TimeSpan Timer { get; set; }
}

public class CreatePomodoroResponse : Response
{
    public CreatePomodoroResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public CreatePomodoroResponse()
    {

    }
}

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
