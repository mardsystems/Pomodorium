using Pomodorium.Enums;

namespace Pomodorium.PomodoroTechnique.Model;

public class Pomodoro : AggregateRoot
{
    public string? Task { get; private set; }

    public TimeSpan Timer { get; private set; }

    public DateTime StartDateTime { get; private set; }

    public PomodoroStateEnum State { get; private set; }

    public Pomodoro(string task, TimeSpan timer, DateTime moment)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        Apply(new PomodoroCreated(Id, task, timer, moment, PomodoroStateEnum.Unknown));
    }

    public void When(PomodoroCreated e)
    {
        Id = e.Id;

        Task = e.Task;

        Timer = e.Timer;

        StartDateTime = e.StartDateTime;

        State = e.State;
    }

    public PomodoroStateEnum GetStateAt(DateTime moment)
    {
        var elapsedTime = moment - StartDateTime;

        if (elapsedTime >= Timer)
        {
            return PomodoroStateEnum.Stopped;
        }
        else
        {
            return PomodoroStateEnum.Running;
        }
    }

    public void RefineTask(string task)
    {
        Apply(new PomodoroTaskRefined(Id, task));
    }

    public void When(PomodoroTaskRefined e)
    {
        Task = e.Task;
    }

    public void Check()
    {
        var state = PomodoroStateEnum.Checked;

        Apply(new PomodoroChecked(Id, state));
    }

    public void When(PomodoroChecked e)
    {
        State = e.State;
    }

    public override void Archive()
    {
        Apply(new PomodoroArchived(Id));
    }

    public void When(PomodoroArchived e)
    {
        base.Archive();
    }

    public Pomodoro() { }
}
