using Pomodorium.Enums;
using System.Reactive.Linq;

namespace Pomodorium.Pages.Pomos;

public class DetailsViewModel
{
    public Guid Id { get; set; }

    public string? Task { get; set; }

    public TimeSpan? Timer { get; set; }

    public TimeSpan? Countdown { get; set; }

    public DateTime? StartDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    public DateTime? StopDate { get; set; }

    public TimeSpan? StopTime { get; set; }

    public PomodoroStateEnum State { get; set; }

    public long Version { get; set; }

    public IObservable<long> CountdownChanges { get; set; }

    private DateTime _stopDateTime;

    public DetailsViewModel(
        Guid id,
        string? task,
        TimeSpan timer,
        DateTime? startDateTime,
        PomodoroStateEnum state,
        long version)
    {
        var now = DateTime.Now;

        Id = id;

        Task = task;

        Timer = timer;

        StartDate = startDateTime.Value.Date;

        StartTime = startDateTime - StartDate;

        _stopDateTime = startDateTime.Value.Add(Timer.Value);

        StopDate = _stopDateTime.Date;

        StopTime = _stopDateTime - StopDate;

        Version = version;

        State = state;

        if (State == PomodoroStateEnum.Checked)
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
        Countdown = _stopDateTime - moment;

        if (Countdown > TimeSpan.Zero)
        {
            State = PomodoroStateEnum.Running;
        }
        else
        {
            Countdown = TimeSpan.Zero;

            State = PomodoroStateEnum.Stopped;
        }
    }

    public DetailsViewModel()
    {

    }
}
