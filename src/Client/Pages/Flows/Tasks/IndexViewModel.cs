﻿using Pomodorium.Modules.Flows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;

namespace Pomodorium.Pages.Flows.Tasks;

public class IndexViewModel
{
    public ObservableCollection<Item>? Items { get; } = new ObservableCollection<Item>();

    public IObservable<EventPattern<NotifyCollectionChangedEventArgs>> ItemsChanged { get; }

    public IndexViewModel()
    {
        ItemsChanged = Observable
            .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(h => Items.CollectionChanged += h, h => Items.CollectionChanged -= h)
            .Throttle(TimeSpan.FromSeconds(1));
    }

    public void Repopulate(IEnumerable<TaskQueryItem> items)
    {
        Items.Clear();

        foreach (var item in items)
        {
            Items.Add(new Item(
                item.Id,
                item.CreationDate,
                item.Description,
                item.TotalHours,
                item.Version));
        }
    }

    public class Item
    {
        public Guid Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public string? Description { get; set; }

        public double TotalHours { get; set; }

        public long Version { get; set; }

        public IObservable<long> BreakCountdownChanges { get; set; }

        public Item(
            Guid id,
            DateTime? creationDate,
            string? description,
            double? totalHours,
            long version)
        {
            var now = DateTime.Now;

            Id = id;

            CreationDate = creationDate;

            Description = description;

            if (totalHours.HasValue)
            {
                TotalHours = Math.Round(totalHours.Value, 1);
            }
            else
            {
                TotalHours = 0;
            }

            Version = version;
        }

        public Item()
        {

        }
    }
}
