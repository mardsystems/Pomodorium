using System.DomainModel;
using System.Runtime.Serialization;

namespace Pomodorium.Modules.Timers;

[DataContract]
public class PomodoroCreated : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public string Description { get; private set; }

    [DataMember(Order = 3)]
    public TimerState State { get; private set; }

    public PomodoroCreated(Guid id, string description, TimerState state)
    {
        Id = id;

        Description = description;

        State = state;
    }

    private PomodoroCreated()
    {

    }
}

[DataContract]
public class PomodoroStarted : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime StartDateTime { get; private set; }

    public PomodoroStarted(Guid id, DateTime startDateTime)
    {
        Id = id;

        StartDateTime = startDateTime;
    }

    private PomodoroStarted()
    {

    }
}

[DataContract]
public class PomodoroDescriptionChanged : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public string? Description { get; private set; }

    public PomodoroDescriptionChanged(Guid id, string description)
    {
        Id = id;

        Description = description;
    }

    private PomodoroDescriptionChanged()
    {

    }
}

[DataContract]
public class PomodoroArchived : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    public PomodoroArchived(Guid id)
    {
        Id = id;
    }

    private PomodoroArchived()
    {

    }
}
