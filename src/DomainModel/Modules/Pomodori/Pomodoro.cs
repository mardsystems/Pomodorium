using System.DomainModel;
using System.DomainModel.EventStore;
using System.Runtime.Serialization;

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

[DataContract]
public class PomodoroId : IIdentity
{
    [DataMember(Order = 1)]
    public string Value { get; private set; }

    public PomodoroId(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Pomodoro-{Value}";
    }

    private PomodoroId()
    {
        
    }
}

[DataContract]
public class PomodoroCreated : Event
{
    [DataMember(Order = 1)]
    public PomodoroId Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime StartDateTime { get; private set; }

    [DataMember(Order = 3)]
    public string? Description { get; private set; }

    public PomodoroCreated(PomodoroId id, DateTime startDateTime, string description)
    {
        Id = id;

        StartDateTime = startDateTime;

        Description = description;
    }

    private PomodoroCreated()
    {
            
    }
}
