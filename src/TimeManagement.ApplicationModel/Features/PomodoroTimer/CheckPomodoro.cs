namespace Pomodorium.Features.PomodoroTimer;

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
