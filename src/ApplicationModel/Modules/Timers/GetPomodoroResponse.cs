namespace Pomodorium.Modules.Timers;

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
