using Pomodorium.Enums;
using System.Runtime.Serialization;

namespace Pomodorium.Models.TaskManagement.Activities;

[DataContract]
public class ActivityCreated : Event
{
    [DataMember(Order = 1)]
    public Guid ActivityId { get; private set; }

    [DataMember(Order = 2)]
    public string ActivityName { get; private set; } = default!;

    [DataMember(Order = 3)]
    public DateTime? StartDateTime { get; private set; }

    [DataMember(Order = 4)]
    public DateTime? StopDateTime { get; private set; }

    [DataMember(Order = 5)]
    public ActivityStateEnum ActivityState { get; set; }

    [DataMember(Order = 6)]
    public TimeSpan? ActivityDuration { get; private set; }

    [DataMember(Order = 7)]
    public string? ActivityDescription { get; private set; }

    public ActivityCreated(
        Guid activityId,
        string activityName,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        ActivityStateEnum state,
        TimeSpan? duration,
        string? description)
    {
        ActivityId = activityId;

        ActivityName = activityName;

        StartDateTime = startDateTime;

        StopDateTime = stopDateTime;

        ActivityState = state;

        ActivityDuration = duration;

        ActivityDescription = description;
    }

    private ActivityCreated() { }
}

[DataContract]
public class ActivityUpdated : Event
{
    [DataMember(Order = 1)]
    public Guid ActivityId { get; private set; }

    [DataMember(Order = 2)]
    public string ActivityName { get; private set; } = default!;

    [DataMember(Order = 3)]
    public DateTime? StartDateTime { get; private set; }

    [DataMember(Order = 4)]
    public DateTime? StopDateTime { get; private set; }

    [DataMember(Order = 5)]
    public ActivityStateEnum ActivityState { get; set; }

    [DataMember(Order = 6)]
    public TimeSpan? ActivityDuration { get; private set; }

    [DataMember(Order = 7)]
    public string? ActivityDescription { get; private set; }

    public ActivityUpdated(
        Guid activityId,
        string activityName,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        ActivityStateEnum state,
        TimeSpan? duration,
        string? description)
    {
        ActivityId = activityId;

        ActivityName = activityName;

        StartDateTime = startDateTime;

        StopDateTime = stopDateTime;

        ActivityState = state;

        ActivityDuration = duration;

        ActivityDescription = description;
    }

    private ActivityUpdated() { }
}

[DataContract]
public class ActivityDeleted : Event
{
    [DataMember(Order = 1)]
    public Guid ActivityId { get; private set; }

    public ActivityDeleted(Guid activityId)
    {
        ActivityId = activityId;
    }

    private ActivityDeleted() { }
}
