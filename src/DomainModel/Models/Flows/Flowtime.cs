﻿using Pomodorium.Enums;

namespace Pomodorium.Models.Flows;

public class Flowtime : AggregateRoot
{
    public Guid TaskId { get; private set; }

    public DateTime? StartDateTime { get; private set; }

    public DateTime? StopDateTime { get; private set; }

    public bool? Interrupted { get; private set; }

    public TimeSpan? Worktime { get; private set; }

    public TimeSpan? Breaktime { get; private set; }

    public TimeSpan? ExpectedDuration { get; private set; }

    public FlowtimeStateEnum? State { get; private set; }

    public Flowtime(Guid id, Pomodorium.Models.Tasks.Task task, AuditInterface auditInterface)
        : base(id, auditInterface)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        var initialState = FlowtimeStateEnum.NotStarted;

        Apply(new FlowtimeCreated(Id, DateTime.Now, task.Id, task.Description, task.Version, initialState));
    }

    public void When(FlowtimeCreated e)
    {
        Id = e.Id;

        CreationDate = e.CreationDate;

        TaskId = e.TaskId;

        State = e.State;
    }

    public Flowtime(Pomodorium.Models.Tasks.Task task, TimeSpan expectedDuration)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        var initialState = FlowtimeStateEnum.NotStarted;

        Apply(new FlowtimeCreated(Id, DateTime.Now, task.Id, task.Description, task.Version, initialState));

        ExpectedDuration = expectedDuration;
    }

    public void Start(DateTime now)
    {
        var newState = FlowtimeStateEnum.Flow;

        Apply(new FlowtimeStarted(Id, TaskId, now, newState));
    }

    public void When(FlowtimeStarted e)
    {
        StartDateTime = e.StartedAt;

        State = e.FlowtimeState;
    }

    public void OnTick(DateTime now)
    {
        Worktime = now - StartDateTime;

        if (Worktime >= ExpectedDuration)
        {
            //State = FlowtimeState.Limbo;
        }
    }

    public void Continue(DateTime now)
    {
        StartDateTime = now;

        State = FlowtimeStateEnum.Flow;
    }

    public void Interrupt(DateTime now)
    {
        if (StartDateTime == null)
        {
            return;
        }

        var worktime = now - StartDateTime;

        var interrupted = true;

        var newState = FlowtimeStateEnum.Stopped;

        TimeSpan breaktime;

        if (worktime <= TimeSpan.FromMinutes(25))
        {
            breaktime = TimeSpan.FromMinutes(5);
        }
        else if (worktime <= TimeSpan.FromMinutes(50))
        {
            breaktime = TimeSpan.FromMinutes(8);
        }
        else if (worktime <= TimeSpan.FromMinutes(90))
        {
            breaktime = TimeSpan.FromMinutes(10);
        }
        else
        {
            breaktime = TimeSpan.FromMinutes(15);
        }

        Apply(new FlowtimeInterrupted(Id, TaskId, now, interrupted, worktime.Value, breaktime, newState));
    }

    public void When(FlowtimeInterrupted e)
    {
        Worktime = e.Worktime;

        StopDateTime = e.StopDateTime;

        Interrupted = e.Interrupted;

        Worktime = e.Worktime;

        Breaktime = e.Breaktime;

        State = e.State;
    }

    public void Stop(DateTime now)
    {
        if (StartDateTime == null)
        {
            return;
        }

        var worktime = now - StartDateTime;

        var interrupted = false;

        var newState = FlowtimeStateEnum.Stopped;

        TimeSpan breaktime;

        if (worktime <= TimeSpan.FromMinutes(25))
        {
            breaktime = TimeSpan.FromMinutes(5);
        }
        else if (worktime <= TimeSpan.FromMinutes(50))
        {
            breaktime = TimeSpan.FromMinutes(8);
        }
        else if (worktime <= TimeSpan.FromMinutes(90))
        {
            breaktime = TimeSpan.FromMinutes(10);
        }
        else
        {
            breaktime = TimeSpan.FromMinutes(15);
        }

        Apply(new FlowtimeStopped(Id, TaskId, now, interrupted, worktime.Value, breaktime, newState));
    }

    public void When(FlowtimeStopped e)
    {
        Worktime = e.Worktime;

        StopDateTime = e.StopDateTime;

        Interrupted = e.Interrupted;

        Worktime = e.Worktime;

        Breaktime = e.Breaktime;

        State = e.State;
    }

    public override void Archive()
    {
        Apply(new FlowtimeArchived(Id, TaskId, Worktime));
    }

    public void When(FlowtimeArchived _)
    {
        base.Archive();
    }

    public Flowtime() { }
}
