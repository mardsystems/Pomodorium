namespace Pomodorium.Modules.Pomos;

public class CheckPomodoroRequest : Request<CheckPomodoroResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}
