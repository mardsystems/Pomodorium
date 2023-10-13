using Pomodorium.EventStore;

namespace Pomodorium.Modules.Pomodori;

public class Pomodoro : AggregateRoot
{
    public PomodoroId Id { get; private set; }

    public DateTime StartDateTime { get; private set; }

    public DateTime? EndDateTime { get; private set; }

    public string? Description { get; private set; }

    public Pomodoro(PomodoroId id, DateTime startDateTime, string description)
    {
        Apply(new PomodoroCreated(id, description));
    }

    public void When(PomodoroCreated e)
    {
        Id = e.Id;

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
    public PomodoroId Id { get; }

    public string? Description { get; }

    public PomodoroCreated(PomodoroId id, string description)
    {
        Id = id;

        Description = description;
    }
}
