namespace Pomodorium.Modules.Timers;

public class PomodoroQueryItem
{
    public Guid Id { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public string? Description { get; set; }
}
