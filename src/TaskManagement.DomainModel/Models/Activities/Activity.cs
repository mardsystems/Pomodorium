using Pomodorium.Enums;

namespace TaskManagement.Models.Activities;

public class Activity : AggregateRoot
{
    public string Name { get; private set; } = default!;

    public DateTime? StartDateTime { get; private set; }

    public DateTime? StopDateTime { get; private set; }

    public ActivityStateEnum State { get; set; }

    public TimeSpan? Duration { get; private set; }

    public string? Description { get; private set; }

    public Activity(
        string name,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        string? description)
    {
        ActivityStateEnum state;

        if (startDateTime.HasValue)
        {
            if (stopDateTime.HasValue)
            {
                state = ActivityStateEnum.Stopped;
            }
            else
            {
                state = ActivityStateEnum.Started;
            }
        }
        else
        {
            state = ActivityStateEnum.NotStarted;
        }

        var duration = stopDateTime - startDateTime;

        Apply(new ActivityCreated(Id, name, startDateTime, stopDateTime, state, duration, description));
    }

    public void When(ActivityCreated e)
    {
        Id = e.ActivityId;

        Name = e.ActivityName;

        StartDateTime = e.StartDateTime;

        StopDateTime = e.StopDateTime;

        State = e.ActivityState;

        Duration = e.ActivityDuration;

        Description = e.ActivityDescription;
    }

    public void Update(
        string name,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        string? description)
    {
        ActivityStateEnum state;

        if (startDateTime.HasValue)
        {
            if (stopDateTime.HasValue)
            {
                state = ActivityStateEnum.Stopped;
            }
            else
            {
                state = ActivityStateEnum.Started;
            }
        }
        else
        {
            state = ActivityStateEnum.NotStarted;
        }

        var duration = stopDateTime - startDateTime;

        Apply(new ActivityUpdated(Id, name, startDateTime, stopDateTime, state, duration, description));
    }

    public void When(ActivityUpdated e)
    {
        Id = e.ActivityId;

        Name = e.ActivityName;

        StartDateTime = e.StartDateTime;

        StopDateTime = e.StopDateTime;

        State = e.ActivityState;

        Duration = e.ActivityDuration;

        Description = e.ActivityDescription;
    }

    public void Delete()
    {
        Apply(new ActivityDeleted(Id));
    }

    public void When(ActivityDeleted _)
    {
        base.Archive();
    }

    public Activity() { }
}
