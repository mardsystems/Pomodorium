using Pomodorium.Modules.Activities;

namespace Pomodorium.Pages.Activities;

public class DetailsViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime? StartDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    public DateTime? StopDate { get; set; }

    public TimeSpan? StopTime { get; set; }

    public ActivityState State { get; set; }

    public TimeSpan? Duration { get; set; }

    public string Description { get; set; }

    public long Version { get; set; }

    public DetailsViewModel(
        Guid id,
        string name,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        ActivityState state,
        TimeSpan? duration,
        string description,
        long version)
    {
        Id = id;

        Name = name;

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

        State = state;

        Duration = duration;

        Description = description;

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
