using Pomodorium.Enums;
using PomodoroTechnique.Features.PomodoroTimer;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;

namespace Pomodorium.Pages.Pomos;

public class IndexViewModel
{
    public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

    public IObservable<EventPattern<NotifyCollectionChangedEventArgs>> ItemsChanged { get; }

    public IObservable<long> CountdownChanges { get; }

    public IndexViewModel()
    {
        ItemsChanged = Observable
            .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(h => Items.CollectionChanged += h, h => Items.CollectionChanged -= h)
            .Throttle(TimeSpan.FromSeconds(1));

        CountdownChanges = Observable.Interval(TimeSpan.FromSeconds(1));

        CountdownChanges.Subscribe(x =>
        {
            OnTick(DateTime.Now);
        });
    }

    private void OnTick(DateTime moment)
    {
        var uncheckedItems = Items.Where(x => x.State != PomodoroStateEnum.Checked);

        foreach (var item in uncheckedItems)
        {
            item.OnTick(moment);
        }
    }

    public void Repopulate(IEnumerable<PomodoroQueryItem> items)
    {
        Items.Clear();

        foreach (var item in items)
        {
            Items.Add(new Item(
                item.Id,
                item.Task,
                item.Timer,
                item.StartDateTime,
                item.State,
                item.Version));
        }
    }

    public class Item
    {
        public Guid Id { get; set; }

        public string Task { get; set; }

        public TimeSpan Timer { get; set; }

        public TimeSpan? Countdown { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? StopDateTime { get; set; }

        public PomodoroStateEnum State { get; set; }

        public long Version { get; set; }

        public IObservable<long> CountdownChanges { get; set; }

        public Item(
            Guid id,
            string task,
            TimeSpan timer,
            DateTime? startDateTime,
            PomodoroStateEnum state,
            long version)
        {
            var now = DateTime.Now;

            Id = id;

            Task = task;

            Timer = timer;

            if (startDateTime == null)
            {
                StartDateTime = null;
            }
            else
            {
                StartDateTime = startDateTime.Value;
            }

            if (StartDateTime == null)
            {
                StopDateTime = null;
            }
            else
            {
                StopDateTime = StartDateTime.Value + Timer;
            }

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

        public void OnTick(DateTime moment)
        {
            if (StopDateTime == null)
            {
                Countdown = null;
            }
            else
            {
                Countdown = StopDateTime - moment;
            }

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

        public Item()
        {
            CountdownChanges = Observable.Empty<long>();
        }
    }
}
