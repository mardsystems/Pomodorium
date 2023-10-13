using System.DomainModel;
using System.DomainModel.EventStore;

namespace Pomodorium.Modules.Pomodori;

public class Pomodoro : AggregateRoot
{
    public PomodoroId Id { get; private set; }

    public DateTime StartDateTime { get; private set; }

    public DateTime? EndDateTime { get; private set; }

    public string? Description { get; private set; }

    public Pomodoro(PomodoroId id, DateTime startDateTime, string description)
    {
        Apply(new PomodoroCreated(id, startDateTime, description));
    }

    public void When(PomodoroCreated e)
    {
        Id = e.Id;

        StartDateTime = e.StartDateTime;

        Description = e.Description;
    }

    public Pomodoro(IEnumerable<Event> events)
    {
        foreach (var @event in events)
        {
            Mutate(@event);
        }
    }
}

[Serializable]
public class PomodoroId : IIdentity
{
    public string Value { get; internal set; }

    public PomodoroId(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Pomodoro-{Value}";
    }

    public PomodoroId()
    {

    }
}

[Serializable]
public class PomodoroCreated : Event
{
    public PomodoroId Id { get; internal set; }

    public DateTime StartDateTime { get; internal set; }

    public string? Description { get; internal set; }

    public PomodoroCreated(PomodoroId id, DateTime startDateTime, string description)
    {
        Id = id;

        StartDateTime = startDateTime;

        Description = description;
    }
}
