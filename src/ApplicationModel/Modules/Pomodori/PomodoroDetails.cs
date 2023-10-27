namespace Pomodorium;

public class PomodoroDetails
{
    public Guid Id { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public string? Description { get; set; }

    public long Version { get; set; }
}
