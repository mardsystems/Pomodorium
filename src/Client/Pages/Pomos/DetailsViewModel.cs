using Pomodorium.Modules.Pomos;
using System.Reactive.Linq;

namespace Pomodorium.Pages.Pomos;

public class DetailsViewModel
{
    public Guid Id { get; set; }

    public string? Task { get; set; }

    public TimeSpan Timer { get; set; }

    public TimeSpan Countdown { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime StopDateTime { get; set; }

    public PomodoroState State { get; set; }

    public long Version { get; set; }

    public IObservable<long> CountdownChanges { get; set; }

    public DetailsViewModel(
        Guid id,
        string? task,
        TimeSpan timer,
        DateTime? startDateTime,
        PomodoroState state,
        long version)
    {
        var now = DateTime.Now;

        Id = id;

        Task = task;

        Timer = timer;

        StartDateTime = startDateTime.Value;

        StopDateTime = StartDateTime + Timer;

        Version = version;

        State = state;

        if (State == PomodoroState.Checked)
        {
            Countdown = TimeSpan.Zero;

            CountdownChanges = Observable.Empty<long>();
        }
        else
        {
            OnTick(now);

            CountdownChanges = Observable.Interval(TimeSpan.FromSeconds(1));

            CountdownChanges.Subscribe(x =>
            {
                OnTick(DateTime.Now);
            });
        }
    }

    private void OnTick(DateTime moment)
    {
        Countdown = StopDateTime - moment;

        if (Countdown > TimeSpan.Zero)
        {
            State = PomodoroState.Running;
        }
        else
        {
            Countdown = TimeSpan.Zero;

            State = PomodoroState.Stopped;
        }
    }

    public DetailsViewModel()
    {

    }
}
