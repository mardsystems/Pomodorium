namespace Pomodorium.Features.PomodoroTimer;

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
