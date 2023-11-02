namespace Pomodorium.Modules.Timers;

public class PutPomodoroRequest : Request<PutPomodoroResponse>
{
    public Guid Id { get; set; }

    public string Description { get; set; }

    public long Version { get; set; }
}
