using Pomodorium.FlowtimeTechnique;

namespace Pomodorium.TimeManagement.FlowTimer;

public class FlowtimeQueryItem
{
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public Guid TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public bool? Interrupted { get; set; }

    public TimeSpan? Worktime { get; set; }

    public TimeSpan? Breaktime { get; set; }

    public TimeSpan? ExpectedDuration { get; set; }

    public FlowtimeState? State { get; set; }

    public long Version { get; set; }
}
