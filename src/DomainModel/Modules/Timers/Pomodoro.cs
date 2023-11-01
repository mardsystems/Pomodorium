using System.DomainModel;

namespace Pomodorium.Modules.Timers;

public class Pomodoro : AggregateRoot
{
    public DateTime StartDateTime { get; private set; }

    public DateTime? EndDateTime { get; private set; }

    public string? Description { get; private set; }

    public Pomodoro(Guid id, DateTime startDateTime, string description)
    {
        Apply(new PomodoroCreated(id, startDateTime, description));
    }

    public void ChangeDescription(string description)
    {
        Apply(new PomodoroDescriptionChanged(Id, description));
    }

    public void Archive()
    {
        Apply(new PomodoroArchived(Id));
    }

    public void When(PomodoroCreated e)
    {
        Id = e.Id;

        StartDateTime = e.StartDateTime;

        Description = e.Description;
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
