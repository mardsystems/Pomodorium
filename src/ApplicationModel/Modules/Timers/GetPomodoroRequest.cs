namespace Pomodorium.Modules.Timers;

public class GetPomodoroRequest : Request<GetPomodoroResponse>
{
    public Guid Id { get; set; }
}
