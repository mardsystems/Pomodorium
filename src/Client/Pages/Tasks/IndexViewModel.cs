using Pomodorium.Enums;
using Pomodorium.Features.TaskManager;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;

namespace Pomodorium.Pages.Tasks;

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
                item.IntegrationType,
                item.IntegrationId,
                item.IntegrationName,
                item.ExternalReference,
                item.HasFocus,
                item.Version));
        }
    }

    public class Item
    {
        public Guid Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public string? Description { get; set; }

        public double TotalHours { get; set; }

        public IntegrationTypeEnum? IntegrationType { get; set; }

        public Guid? IntegrationId { get; set; }

        public string? IntegrationName { get; set; }

        public string? ExternalReference { get; set; }

        public bool? HasFocus { get; set; }

        public long Version { get; set; }

        public IObservable<long> BreakCountdownChanges { get; set; }

        public Item(
            Guid id,
            DateTime? creationDate,
            string? description,
            double? totalHours,
            IntegrationTypeEnum? integrationType,
            Guid? integrationId,
            string? integrationName,
            string? externalReference,
            bool? hasFocus,
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

            IntegrationType = integrationType;

            IntegrationId = integrationId;

            IntegrationName = integrationName;

            ExternalReference = externalReference;

            HasFocus = hasFocus;

            Version = version;
        }

        public Item()
        {

        }
    }
}
