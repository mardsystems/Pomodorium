using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PomodoroTechnique.Features.PomodoroTimer;

namespace Pomodorium.Pages.Pomos;

public partial class Index
{
    [Inject]
    private IMediator Mediator { get; set; }

    private readonly IndexViewModel _model = new();

    private MudTable<IndexViewModel.Item> _mudTable;

    private int _selectedRowNumber = -1;

    protected override async Task OnInitializedAsync()
    {
        _model.ItemsChanged.Subscribe((x) =>
        {
            StateHasChanged();
        });

        _model.CountdownChanges.Subscribe(x =>
        {
            StateHasChanged();
        });

        // EventHubClient.Notification
        //     .Throttle(TimeSpan.FromSeconds(1))
        //     .Subscribe(async (x) =>
        //     {
        //         await Refresh();
        //     });

        await Refresh();
    }

    private async Task Add()
    {
        Navigation.NavigateTo($"/pomos/new");

        await Task.CompletedTask;
    }

    private async Task Refresh()
    {
        var request = new PomodoroQueryRequest { };

        var response = await Mediator.Send<PomodoroQueryResponse>(request);

        _model.Repopulate(response.PomodoroQueryItems);
    }

    //private async Task Add2()
    //{
    //    var now = DateTime.Now;

    //    var request = new PomodoroCreationRequest
    //    {
    //        Timer = TimeSpan.FromMinutes(25),
    //        Task = now.ToString()
    //    };

    //    var _ = await Mediator.Send<PomodoroCreationResponse>(request);

    //    await Refresh();
    //}

    private async Task Archive(Guid id, long version)
    {
        var request = new PomodoroArchivingRequest
        {
            Id = id,
            Version = version
        };

        var _ = await Mediator.Send<PomodoroArchivingResponse>(request);

        await Refresh();
    }

    private static void RowClickEvent(TableRowClickEventArgs<IndexViewModel.Item> e)
    {
        // clickedEvents.Add("Row has been clicked");
    }

    private string SelectedRowClassFunc(IndexViewModel.Item element, int rowNumber)
    {
        if (_selectedRowNumber == rowNumber)
        {
            _selectedRowNumber = -1;
            // clickedEvents.Add("Selected Row: None");
            return string.Empty;
        }
        else if (_mudTable.SelectedItem != null && _mudTable.SelectedItem.Equals(element))
        {
            _selectedRowNumber = rowNumber;
            // clickedEvents.Add($"Selected Row: {rowNumber}");

            Navigation.NavigateTo($"/pomos/{_mudTable.SelectedItem.Id}");

            return "selected";
        }
        else
        {
            return string.Empty;
        }
    }
}
