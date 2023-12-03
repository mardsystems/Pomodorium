﻿using Pomodorium.Enums;
using System.Runtime.Serialization;

namespace Pomodorium.FlowtimeTechnique.Model;

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
    public FlowtimeStateEnum State { get; private set; }

    public FlowtimeCreated(Guid id, DateTime creationDate, Guid taskId, string taskDescription, long taskVersion, FlowtimeStateEnum state)
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
    public Guid TaskId { get; private set; }

    [DataMember(Order = 3)]
    public DateTime StartDateTime { get; private set; }

    [DataMember(Order = 4)]
    public FlowtimeStateEnum State { get; private set; }

    public FlowtimeStarted(Guid id, Guid taskId, DateTime startDateTime, FlowtimeStateEnum state)
    {
        Id = id;

        TaskId = taskId;

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
    public Guid TaskId { get; private set; }

    [DataMember(Order = 3)]
    public DateTime StopDateTime { get; private set; }

    [DataMember(Order = 4)]
    public bool Interrupted { get; private set; }

    [DataMember(Order = 5)]
    public TimeSpan Worktime { get; private set; }

    [DataMember(Order = 6)]
    public TimeSpan Breaktime { get; private set; }

    [DataMember(Order = 7)]
    public FlowtimeStateEnum State { get; private set; }

    public FlowtimeInterrupted(
        Guid id,
        Guid taskId,
        DateTime stopDateTime,
        bool interrupted,
        TimeSpan worktime,
        TimeSpan breaktime,
        FlowtimeStateEnum state)
    {
        Id = id;

        TaskId = taskId;

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
    public Guid TaskId { get; private set; }

    [DataMember(Order = 3)]
    public DateTime StopDateTime { get; private set; }

    [DataMember(Order = 4)]
    public bool Interrupted { get; private set; }

    [DataMember(Order = 5)]
    public TimeSpan Worktime { get; private set; }

    [DataMember(Order = 6)]
    public TimeSpan Breaktime { get; private set; }

    [DataMember(Order = 7)]
    public FlowtimeStateEnum State { get; private set; }

    public FlowtimeStopped(
        Guid id,
        Guid taskId,
        DateTime stopDateTime,
        bool interrupted,
        TimeSpan worktime,
        TimeSpan breaktime,
        FlowtimeStateEnum state)
    {
        Id = id;

        TaskId = taskId;

        StopDateTime = stopDateTime;

        Interrupted = interrupted;

        Worktime = worktime;

        Breaktime = breaktime;

        State = state;
    }

    private FlowtimeStopped() { }
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
