using Pomodorium.Modules.Pomos;
using System.DomainModel;

namespace Pomodorium.Modules.Flows;

public class Flowtime : AggregateRoot
{
    public Guid TaskId { get; private set; }

    public DateTime? StartDateTime { get; private set; }

    public DateTime? StopDateTime { get; private set; }

    public bool? Interrupted { get; private set; }

    public TimeSpan? Worktime { get; private set; }

    public TimeSpan? Breaktime { get; private set; }

    public TimeSpan? ExpectedDuration { get; private set; }

    public FlowtimeState? State { get; private set; }

    public Flowtime(Task task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        var initialState = FlowtimeState.NotStarted;

        Apply(new FlowtimeCreated(Id, task.Id, task.Description, initialState));
    }

    public void When(FlowtimeCreated e)
    {
        Id = e.Id;

        TaskId = e.TaskId;

        State = e.State;
    }

    public Flowtime(Task task, TimeSpan expectedDuration)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        var initialState = FlowtimeState.NotStarted;

        Apply(new FlowtimeCreated(Id, task.Id, task.Description, initialState));

        ExpectedDuration = expectedDuration;
    }

    public void Start(DateTime now)
    {
        var newState = FlowtimeState.Flow;

        Apply(new FlowtimeStarted(Id, now, newState));
    }

    public void When(FlowtimeStarted e)
    {
        StartDateTime = e.StartDateTime;

        State = e.State;
    }

    public void OnTick(DateTime now)
    {
        Worktime = now - StartDateTime;

        if (Worktime >= ExpectedDuration)
        {
            State = FlowtimeState.Limbo;
        }
    }

    public void Continue(DateTime now)
    {
        StartDateTime = now;

        State = FlowtimeState.Flow;
    }

    public void Interrupt(DateTime now)
    {
        Worktime = now - StartDateTime;

        Interrupted = true;

        StopDateTime = now;

        State = FlowtimeState.Finished;

        if (Worktime <= TimeSpan.FromMinutes(25))
        {
            Breaktime = TimeSpan.FromMinutes(5);
        }
        else if (Worktime <= TimeSpan.FromMinutes(50))
        {
            Breaktime = TimeSpan.FromMinutes(8);
        }
        else if (Worktime <= TimeSpan.FromMinutes(90))
        {
            Breaktime = TimeSpan.FromMinutes(10);
        }
        else
        {
            Breaktime = TimeSpan.FromMinutes(15);
        }
    }

    public void Stop(DateTime now)
    {
        var worktime = now - StartDateTime;

        var interrupted = false;

        var newState = FlowtimeState.Finished;

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

        Apply(new FlowtimeStopped(Id, now, interrupted, worktime.Value, breaktime, newState));
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
        Apply(new FlowtimeArchived(Id));
    }

    public void When(FlowtimeArchived e)
    {
        base.Archive();
    }

    public Flowtime() { }
}
