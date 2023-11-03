using System.DomainModel;
using System.Runtime.Serialization;

namespace Pomodorium.Modules.Activities;

[DataContract]
public class ActivityCreated : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public string Name { get; private set; }

    [DataMember(Order = 3)]
    public DateTime? StartDateTime { get; private set; }

    [DataMember(Order = 4)]
    public DateTime? StopDateTime { get; private set; }

    [DataMember(Order = 5)]
    public ActivityState State { get; set; }

    [DataMember(Order = 6)]
    public TimeSpan? Duration { get; private set; }

    [DataMember(Order = 7)]
    public string Description { get; private set; }

    public ActivityCreated(
        Guid id,
        string name,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        ActivityState state,
        TimeSpan? duration,
        string description)
    {
        Id = id;

        Name = name;

        StartDateTime = startDateTime;

        StopDateTime = stopDateTime;

        State = state;

        Duration = duration;

        Description = description;
    }

    private ActivityCreated() { }
}

[DataContract]
public class ActivityUpdated : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    [DataMember(Order = 2)]
    public string Name { get; private set; }

    [DataMember(Order = 3)]
    public DateTime? StartDateTime { get; private set; }

    [DataMember(Order = 4)]
    public DateTime? StopDateTime { get; private set; }

    [DataMember(Order = 5)]
    public ActivityState State { get; set; }

    [DataMember(Order = 6)]
    public TimeSpan? Duration { get; private set; }

    [DataMember(Order = 7)]
    public string Description { get; private set; }

    public ActivityUpdated(
        Guid id,
        string name,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        ActivityState state,
        TimeSpan? duration,
        string description)
    {
        Id = id;

        Name = name;

        StartDateTime = startDateTime;

        StopDateTime = stopDateTime;

        State = state;

        Duration = duration;

        Description = description;
    }

    private ActivityUpdated() { }
}

[DataContract]
public class ActivityDeleted : Event
{
    [DataMember(Order = 1)]
    public Guid Id { get; private set; }

    public ActivityDeleted(Guid id)
    {
        Id = id;
    }

    private ActivityDeleted() { }
}
