using Pomodorium.Modules.Flows;

namespace Pomodorium.Pages.Flows;

public class DetailsViewModel
{
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public long TaskVersion { get; set; }

    public DateTime? StartDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    public DateTime? StopDate { get; set; }

    public TimeSpan? StopTime { get; set; }

    public bool? Interrupted { get; set; }

    public TimeSpan? Worktime { get; set; }

    public TimeSpan? Breaktime { get; set; }

    public FlowtimeState? State { get; set; }

    public long Version { get; set; }

    public DetailsViewModel(
        Guid id,
        Guid taskId,
        string? taskDescription,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        bool? interrupted,
        TimeSpan? worktime,
        TimeSpan? breaktime,
        FlowtimeState? state,
        long version)
    {
        Id = id;

        TaskId = taskId;

        TaskDescription = taskDescription;

        if (startDateTime.HasValue)
        {
            StartDate = startDateTime.Value.Date;

            StartTime = startDateTime - StartDate;
        }

        if (stopDateTime.HasValue)
        {
            StopDate = stopDateTime.Value.Date;

            StopTime = stopDateTime - StopDate;
        }

        Interrupted = interrupted;

        Worktime = worktime;

        Breaktime = breaktime;

        State = state;

        Version = version;
    }

    public DateTime? GetStartDateTime()
    {
        if (StartDate.HasValue)
        {
            if (StartTime.HasValue)
            {
                return StartDate.Value.Add(StartTime.Value);
            }
            else
            {
                return StartDate.Value;
            }
        }
        else
        {
            return null;
        }
    }

    public DateTime? GetStopDateTime()
    {
        if (StopDate.HasValue)
        {
            if (StopTime.HasValue)
            {
                return StopDate.Value.Add(StopTime.Value);
            }
            else
            {
                return StopDate.Value;
            }
        }
        else
        {
            return null;
        }
    }

    public DetailsViewModel()
    {

    }
}
