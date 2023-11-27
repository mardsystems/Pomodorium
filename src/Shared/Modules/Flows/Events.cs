using System.DomainModel;
using System.Runtime.Serialization;

namespace Pomodorium.Modules.Flows;

[DataContract]
public class TaskCreated : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime CreationDate { get; private set; }

    [DataMember(Order = 3)]
    public string Description { get; private set; }

    public TaskCreated(Guid id, DateTime creationDate, string description)
    {
        Id = id;

        CreationDate = creationDate;

        Description = description;
    }

    private TaskCreated() { }
}

[DataContract]
public class FlowtimeCreated : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime CreationDate { get; private set; }

    [DataMember(Order = 3)]
    public Guid TaskId { get; private set; }

    [DataMember(Order = 4)]
    public string TaskDescription { get; private set; }

    [DataMember(Order = 5)]
    public long TaskVersion { get; private set; }

    [DataMember(Order = 6)]
    public FlowtimeState State { get; private set; }

    public FlowtimeCreated(Guid id, DateTime creationDate, Guid taskId, string taskDescription, long taskVersion, FlowtimeState state)
    {
        Id = id;

        CreationDate = creationDate;

        TaskId = taskId;

        TaskDescription = taskDescription;

        TaskVersion = taskVersion;

        State = state;
    }

    private FlowtimeCreated() { }
}

[DataContract]
public class FlowtimeStarted : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime StartDateTime { get; private set; }

    [DataMember(Order = 3)]
    public FlowtimeState State { get; private set; }

    public FlowtimeStarted(Guid id, DateTime startDateTime, FlowtimeState state)
    {
        Id = id;

        StartDateTime = startDateTime;

        State = state;
    }

    private FlowtimeStarted() { }
}

[DataContract]
public class FlowtimeInterrupted : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime StopDateTime { get; private set; }

    [DataMember(Order = 3)]
    public bool Interrupted { get; private set; }

    [DataMember(Order = 4)]
    public TimeSpan Worktime { get; private set; }

    [DataMember(Order = 5)]
    public TimeSpan Breaktime { get; private set; }

    [DataMember(Order = 6)]
    public FlowtimeState State { get; private set; }

    public FlowtimeInterrupted(
        Guid id,
        DateTime stopDateTime,
        bool interrupted,
        TimeSpan worktime,
        TimeSpan breaktime,
        FlowtimeState state)
    {
        Id = id;

        StopDateTime = stopDateTime;

        Interrupted = interrupted;

        Worktime = worktime;

        Breaktime = breaktime;

        State = state;
    }

    private FlowtimeInterrupted() { }
}

[DataContract]
public class FlowtimeStopped : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public DateTime StopDateTime { get; private set; }

    [DataMember(Order = 3)]
    public bool Interrupted { get; private set; }

    [DataMember(Order = 4)]
    public TimeSpan Worktime { get; private set; }

    [DataMember(Order = 5)]
    public TimeSpan Breaktime { get; private set; }

    [DataMember(Order = 6)]
    public FlowtimeState State { get; private set; }

    public FlowtimeStopped(
        Guid id,
        DateTime stopDateTime,
        bool interrupted,
        TimeSpan worktime,
        TimeSpan breaktime,
        FlowtimeState state)
    {
        Id = id;

        StopDateTime = stopDateTime;

        Interrupted = interrupted;

        Worktime = worktime;

        Breaktime = breaktime;

        State = state;
    }

    private FlowtimeStopped() { }
}

[DataContract]
public class TaskDescriptionChanged : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public string? Description { get; private set; }

    public TaskDescriptionChanged(Guid id, string description)
    {
        Id = id;

        Description = description;
    }

    private TaskDescriptionChanged() { }
}

[DataContract]
public class TaskArchived : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    public TaskArchived(Guid id)
    {
        Id = id;
    }

    private TaskArchived() { }
}

[DataContract]
public class FlowtimeArchived : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    public FlowtimeArchived(Guid id)
    {
        Id = id;
    }

    private FlowtimeArchived() { }
}
