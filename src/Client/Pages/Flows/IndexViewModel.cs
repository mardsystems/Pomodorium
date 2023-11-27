using Pomodorium.Modules.Flows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;

namespace Pomodorium.Pages.Flows;

public class IndexViewModel
{
    public ObservableCollection<Item>? Items { get; } = new ObservableCollection<Item>();

    public IObservable<EventPattern<NotifyCollectionChangedEventArgs>> ItemsChanged { get; }

    public IObservable<long> BreakCountdownChanges { get; }

    public IndexViewModel()
    {
        ItemsChanged = Observable
            .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(h => Items.CollectionChanged += h, h => Items.CollectionChanged -= h)
            .Throttle(TimeSpan.FromSeconds(1));

        BreakCountdownChanges = Observable.Interval(TimeSpan.FromSeconds(1));

        BreakCountdownChanges.Subscribe(x =>
        {
            OnTick(DateTime.Now);
        });
    }

    private void OnTick(DateTime moment)
    {
        var uncheckedItems = Items.Where(x => x.State != FlowtimeState.NotStarted);

        foreach (var item in uncheckedItems)
        {
            item.OnTick(moment);
        }
    }

    public void Repopulate(IEnumerable<FlowtimeQueryItem> items)
    {
        Items.Clear();

        foreach (var item in items)
        {
            Items.Add(new Item(
                item.Id,
                item.CreationDate,
                item.TaskId,
                item.TaskDescription,
                item.StartDateTime,
                item.StopDateTime,
                item.Interrupted,
                item.Worktime,
                item.Breaktime,
                item.State,
                item.Version));
        }
    }

    public class Item
    {
        public Guid Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public Guid TaskId { get; set; }

        public string? TaskDescription { get; set; }

        public long TaskVersion { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? StopDateTime { get; set; }

        public bool? Interrupted { get; set; }

        public TimeSpan? Worktime { get; set; }

        public TimeSpan? WorkTimer { get; set; }

        public string TimerText { get; set; }

        public TimeSpan? Breaktime { get; set; }

        public TimeSpan? BreakCountdown { get; set; }

        public FlowtimeState? State { get; set; }

        public long Version { get; set; }

        public IObservable<long> BreakCountdownChanges { get; set; }

        public Item(
            Guid id,
            DateTime? creationDate,
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

            CreationDate = creationDate;

            TaskId = taskId;

            TaskDescription = taskDescription;

            StartDateTime = startDateTime;

            StopDateTime = stopDateTime;

            Interrupted = interrupted;

            if (worktime.HasValue)
            {
                Worktime = new TimeSpan(worktime.Value.Ticks - (worktime.Value.Ticks % 10000000));

                WorkTimer = new TimeSpan(worktime.Value.Ticks - (worktime.Value.Ticks % 10000000));
            }
            else
            {
                Worktime = null;

                WorkTimer = null;
            }

            Breaktime = breaktime;

            State = state;

            Version = version;

            State = state;

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

        public void OnTick(DateTime moment)
        {
            if (StopDateTime.HasValue && Breaktime.HasValue)
            {
                var breakCountdown = StopDateTime.Value.Add(Breaktime.Value) - moment;

                BreakCountdown = new TimeSpan(breakCountdown.Ticks - (breakCountdown.Ticks % 10000000));
            }
        }

        public Item()
        {

        }
    }
}
