using System.DomainModel;

namespace Pomodorium.Modules.Flows;

public class Flowtime : AggregateRoot
{
    public Task Task { get; private set; }

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

        Task = task;
    }

    public Flowtime(Task task, TimeSpan expectedDuration)
    {
        Task = task;

        ExpectedDuration = expectedDuration;
    }

    public void Start(DateTime now)
    {
        StartDateTime = now;

        State = FlowtimeState.Flow;
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
        Worktime = now - StartDateTime;

        StopDateTime = now;

        Interrupted = false;

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
}
