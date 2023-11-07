﻿using Pomodorium.Modules.Flows;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;

namespace Pomodorium.Pages.Flows;

public class DetailsViewModel
{
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }

    public string? TaskDescription { get; set; }

    public long TaskVersion { get; set; }

    public DateTime? StartDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    public DateTime? StopDate { get; set; }

    public TimeSpan? StopTime { get; set; }

    public bool? Interrupted { get; set; }

    public TimeSpan? Worktime { get; set; }

    public TimeSpan WorkTimer { get; set; }

    public TimeSpan? Breaktime { get; set; }

    //[DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}")]
    public TimeSpan? BreakCountdown { get; set; }

    public FlowtimeState? State { get; set; }

    public long Version { get; set; }

    public IObservable<long> BreakCountdownChanges { get; set; }

    public DetailsViewModel(
        Guid id,
        Guid taskId,
        string? taskDescription,
        DateTime? startDateTime,
        DateTime? stopDateTime,
        bool? interrupted,
        TimeSpan? worktime,
        TimeSpan? breaktime,
        FlowtimeState? state,
        long version)
    {
        var now = DateTime.Now;

        Id = id;

        TaskId = taskId;

        TaskDescription = taskDescription;

        if (startDateTime.HasValue)
        {
            StartDate = startDateTime.Value.Date;

            StartTime = startDateTime - StartDate;
        }

        if (stopDateTime.HasValue)
        {
            StopDate = stopDateTime.Value.Date;

            StopTime = stopDateTime - StopDate;
        }

        Interrupted = interrupted;

        Worktime = worktime;

        Breaktime = breaktime;

        State = state;

        Version = version;

        if (now - stopDateTime > Breaktime)
        {
            BreakCountdown = TimeSpan.Zero;

            BreakCountdownChanges = Observable.Empty<long>();
        }
        else
        {
            OnTick(now);

            BreakCountdownChanges = Observable.Interval(TimeSpan.FromSeconds(1));

            BreakCountdownChanges.Subscribe(x =>
            {
                OnTick(DateTime.Now);
            });
        }
    }

    public DateTime? GetStartDateTime()
    {
        if (StartDate.HasValue)
        {
            if (StartTime.HasValue)
            {
                return StartDate.Value.Add(StartTime.Value);
            }
            else
            {
                return StartDate.Value;
            }
        }
        else
        {
            return null;
        }
    }

    public DateTime? GetStopDateTime()
    {
        if (StopDate.HasValue)
        {
            if (StopTime.HasValue)
            {
                return StopDate.Value.Add(StopTime.Value);
            }
            else
            {
                return StopDate.Value;
            }
        }
        else
        {
            return null;
        }
    }

    private void OnTick(DateTime moment)
    {
        if (GetStopDateTime().HasValue && Breaktime.HasValue)
        {
            var breakCountdown = GetStopDateTime().Value.Add(Breaktime.Value) - moment;

            BreakCountdown = new TimeSpan(breakCountdown.Ticks - (breakCountdown.Ticks % 10000000));
        }
    }

    public DetailsViewModel()
    {

    }
}
