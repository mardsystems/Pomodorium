namespace Pomodorium.TimeManagement.PomodoroTimer;

public class GetPomodoroRequest : Request<GetPomodoroResponse>
{
    public Guid Id { get; set; }
}

public class GetPomodoroResponse : Response
{
    public GetPomodoroResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PomodoroDetails PomodoroDetails { get; set; }

    public GetPomodoroResponse()
    {

    }
}
