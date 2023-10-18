namespace Pomodorium;

public class DeletePomodoroResponse : Response
{
    public DeletePomodoroResponse(Guid correlationId)
        : base(correlationId)
    {

    }
    
    public PomodoroDetails PomodoroDetails { get; set; }
}
