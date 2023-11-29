namespace Pomodorium.Features.PomodoroTimer;

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
