using System.DomainModel;
using System.Runtime.Serialization;

namespace Pomodorium.Modules.Pomos;

[DataContract]
public class PomodoroCreated : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public string Task { get; private set; }

    [DataMember(Order = 3)]
    public TimeSpan Timer { get; private set; }

    [DataMember(Order = 4)]
    public DateTime StartDateTime { get; private set; }

    [DataMember(Order = 5)]
    public PomodoroState State { get; private set; }

    public PomodoroCreated(Guid id, string task, TimeSpan timer, DateTime startDateTime, PomodoroState state)
    {
        Id = id;

        Task = task;

        Timer = timer;

        StartDateTime = startDateTime;

        State = state;
    }

    private PomodoroCreated() { }
}

[DataContract]
public class PomodoroRinged : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime StopDateTime { get; private set; }

    [DataMember(Order = 5)]
    public PomodoroState State { get; private set; }

    public PomodoroRinged(Guid id, DateTime stopDateTime, PomodoroState state)
    {
        Id = id;

        StopDateTime = stopDateTime;

        State = state;
    }

    private PomodoroRinged() { }
}

[DataContract]
public class PomodoroChecked : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public PomodoroState State { get; private set; }

    public PomodoroChecked(Guid id, PomodoroState state)
    {
        Id = id;

        State = state;
    }

    private PomodoroChecked() { }
}

[DataContract]
public class PomodoroTaskRefined : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public string Task { get; private set; }

    public PomodoroTaskRefined(Guid id, string task)
    {
        Id = id;

        Task = task;
    }

    private PomodoroTaskRefined() { }
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

    private PomodoroArchived() { }
}
