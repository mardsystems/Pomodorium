namespace Pomodorium.Modules.Timers;

public class GetTimersResponse : Response
{
    public GetTimersResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public IEnumerable<PomodoroQueryItem> PomodoroQueryItems { get; set; }

    public GetTimersResponse()
    {

    }
}
