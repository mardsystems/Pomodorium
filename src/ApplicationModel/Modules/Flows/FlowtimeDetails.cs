using System.ComponentModel.DataAnnotations;

namespace Pomodorium.Modules.Flows;

public class FlowtimeDetails
{
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public long TaskVersion { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public bool? Interrupted { get; set; }

    [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}")]
    public TimeSpan? Worktime { get; set; }

    public TimeSpan? Breaktime { get; set; }

    public TimeSpan? ExpectedDuration { get; set; }

    public FlowtimeState? State { get; set; }

    public long Version { get; set; }
}
