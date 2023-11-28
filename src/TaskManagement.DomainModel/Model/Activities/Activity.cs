namespace Pomodorium.TaskManagement.Model.Activities;

public class Activity : AggregateRoot
{
    public string Name { get; private set; }

    public DateTime? StartDateTime { get; private set; }

    public DateTime? StopDateTime { get; private set; }

    public ActivityState State { get; set; }

    public TimeSpan? Duration { get; private set; }

    public string Description { get; private set; }

    public Activity(
        string name,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        string description)
    {
        ActivityState state;

        if (startDateTime.HasValue)
        {
            if (stopDateTime.HasValue)
            {
                state = ActivityState.Stopped;
            }
            else
            {
                state = ActivityState.Started;
            }
        }
        else
        {
            state = ActivityState.NotStarted;
        }

        var duration = stopDateTime - startDateTime;

        Apply(new ActivityCreated(Id, name, startDateTime, stopDateTime, state, duration, description));
    }

    public void When(ActivityCreated e)
    {
        Id = e.Id;

        Name = e.Name;

        StartDateTime = e.StartDateTime;

        StopDateTime = e.StopDateTime;

        State = e.State;

        Duration = e.Duration;

        Description = e.Description;
    }

    public void Update(
        string name,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        string description)
    {
        ActivityState state;

        if (startDateTime.HasValue)
        {
            if (stopDateTime.HasValue)
            {
                state = ActivityState.Stopped;
            }
            else
            {
                state = ActivityState.Started;
            }
        }
        else
        {
            state = ActivityState.NotStarted;
        }

        var duration = stopDateTime - startDateTime;

        Apply(new ActivityUpdated(Id, name, startDateTime, stopDateTime, state, duration, description));
    }

    public void When(ActivityUpdated e)
    {
        Id = e.Id;

        Name = e.Name;

        StartDateTime = e.StartDateTime;

        StopDateTime = e.StopDateTime;

        State = e.State;

        Duration = e.Duration;

        Description = e.Description;
    }

    public void Delete()
    {
        Apply(new ActivityDeleted(Id));
    }

    public void When(ActivityDeleted e)
    {
        base.Archive();
    }

    public Activity() { }
}
