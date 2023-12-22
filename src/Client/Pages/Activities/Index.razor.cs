using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pomodorium.Features.ActivityManager;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;

namespace Pomodorium.Pages.Activities;

public partial class Index
{
    [Inject]
    private IMediator Mediator { get; set; }

    private readonly ObservableCollection<ActivityQueryItem> _items = new();

    private MudTable<ActivityQueryItem> _mudTable = default!;

    private int selectedRowNumber = -1;

    protected override async Task OnInitializedAsync()
    {
        var x = Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(h => _items.CollectionChanged += h, h => _items.CollectionChanged -= h)
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe((x) =>
            {
                StateHasChanged();
            });

        // var y = EventHubClient.Notification
        //     .Throttle(TimeSpan.FromSeconds(1))
        //     .Subscribe(async (x) =>
        //     {
        //         await Refresh();
        //     });

        await Refresh();
    }

    // protected override async Tasks.Task OnAfterRenderAsync(bool firstRender)
    // {
    //     await base.OnAfterRenderAsync(firstRender);

    //     if (firstRender)
    //     {
    //         await _mudDataGrid.SetSortAsync("CreationDate", SortDirection.Descending, (x) => x.CreationDate);
    //     }
    // }

    private async Task Refresh()
    {
        var request = new ActivityQueryRequest { };

        var response = await Mediator.Send<ActivityQueryResponse>(request);

        _items.Clear();

        foreach (var item in response.ActivityQueryItems)
        {
            _items.Add(item);
        }
    }

    private async Task Add()
    {
        Navigation.NavigateTo($"/activities/new");

        await Task.CompletedTask;
    }

    //private async Task Add2()
    //{
    //    var now = DateTime.Now;

    //    var request = new ActivityCreationRequest
    //    {
    //        Name = "",
    //        StartDateTime = now,
    //        Description = Guid.NewGuid().ToString()
    //    };

    //    var _ = await Mediator.Send<ActivityCreationResponse>(request);

    //    await Refresh();
    //}

    //private async Task Delete(Guid id, long version)
    //{
    //    var request = new ActivityDeletionRequest
    //    {
    //        Id = id,
    //        Version = version
    //    };

    //    var _ = await Mediator.Send<ActivityDeletionResponse>(request);

    //    await Refresh();
    //}

    private static void RowClickEvent(TableRowClickEventArgs<ActivityQueryItem> e)
    {
        // clickedEvents.Add("Row has been clicked");
    }

    private string SelectedRowClassFunc(ActivityQueryItem element, int rowNumber)
    {
        if (selectedRowNumber == rowNumber)
        {
            selectedRowNumber = -1;
            // clickedEvents.Add("Selected Row: None");
            return string.Empty;
        }
        else if (_mudTable.SelectedItem != null && _mudTable.SelectedItem.Equals(element))
        {
            selectedRowNumber = rowNumber;
            // clickedEvents.Add($"Selected Row: {rowNumber}");

            Navigation.NavigateTo($"/activities/{_mudTable.SelectedItem.Id}");

            return "selected";
        }
        else
        {
            return string.Empty;
        }
    }
}
