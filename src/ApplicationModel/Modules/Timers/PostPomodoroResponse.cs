namespace Pomodorium.Modules.Timers;

public class PostPomodoroResponse : Response
{
    public PostPomodoroResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PostPomodoroResponse()
    {
        
    }
}
