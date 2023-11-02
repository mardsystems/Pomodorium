using System.DomainModel;

namespace Pomodorium.Modules.Timers;

public class Pomodoro : AggregateRoot
{
    public DateTime? StartDateTime { get; private set; }

    public DateTime? EndDateTime { get; private set; }

    public string? Description { get; private set; }

    public TimerState State { get; private set; }

    public Pomodoro(Guid id, string description)
    {
        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        Apply(new PomodoroCreated(id, description, State));
    }

    public void ChangeDescription(string description)
    {
        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        Apply(new PomodoroDescriptionChanged(Id, description));
    }

    public void Start()
    {
        var now = DateTime.Now;

        Apply(new PomodoroStarted(Id, now));
    }

    public void Pause()
    {
        State = TimerState.Paused;
    }

    public void Stop()
    {
        State = TimerState.Stopped;
    }

    public void Archive()
    {
        Apply(new PomodoroArchived(Id));
    }

    public void When(PomodoroCreated e)
    {
        Id = e.Id;

        Description = e.Description;
    }

    public void When(PomodoroStarted e)
    {
        Id = e.Id;

        StartDateTime = e.StartDateTime;

        State = TimerState.Started;
    }

    public void When(PomodoroDescriptionChanged e)
    {
        Description = e.Description;
    }

    public void When(PomodoroArchived e)
    {

    }

    public Pomodoro()
    {

    }
}

public class Interval
{
    public DateTime StartDateTime { get; private set; }

    public DateTime? EndDateTime { get; private set; }

    public string? Description { get; private set; }
}
