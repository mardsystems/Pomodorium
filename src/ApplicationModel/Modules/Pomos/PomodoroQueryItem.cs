namespace Pomodorium.Modules.Pomos;

public class PomodoroQueryItem
{
    public Guid Id { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public TimerState State { get; set; }

    public string? Description { get; set; }

    public long Version { get; set; }
}
