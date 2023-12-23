using MediatR;
using Microsoft.AspNetCore.Components;
using Pomodorium.Features.FlowTimer;

namespace Pomodorium.Pages.Flows;

public partial class Index
{
    [Inject]
    private IMediator Mediator { get; set; }

    private readonly IndexViewModel _model = new();

    private bool _loading;

    private bool _dense = false;

    private string _searchString;

    private HashSet<IndexViewModel.Item> _selectedItems = new();

    protected override async Task OnInitializedAsync()
    {
        _model.ItemsChanged.Subscribe((x) =>
        {
            StateHasChanged();
        });

        _model.BreakCountdownChanges.Subscribe(x =>
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

    private async Task Refresh()
    {
        _loading = true;

        var request = new FlowtimeQueryRequest();

        var response = await Mediator.Send<FlowtimeQueryResponse>(request);

        _model.Repopulate(response.FlowtimeQueryItems);

        _loading = false;
    }

    private async Task Add()
    {
        Navigation.NavigateTo($"/flows/new");

        await Task.CompletedTask;
    }

    private async Task Details(Guid id)
    {
        Navigation.NavigateTo($"/flows/{id}");

        await Task.CompletedTask;
    }

    private async Task Continue(Guid taskId)
    {
        Navigation.NavigateTo($"/flows/continuation?TaskId={taskId}");

        await Task.CompletedTask;
    }

    private async Task Start(Guid id, long version)
    {
        _loading = true;

        var request = new FlowtimeStartRequest
        {
            FlowtimeId = id,
            FlowtimeVersion = version
        };

        var _ = await Mediator.Send<FlowtimeStartResponse>(request);

        await Refresh();

        _loading = false;
    }

    private async Task Interrupt(Guid id, long version)
    {
        _loading = true;

        var request = new FlowtimeInterruptionRequest
        {
            FlowtimeId = id,
            FlowtimeVersion = version
        };

        var _ = await Mediator.Send<FlowtimeInterruptionResponse>(request);

        await Refresh();

        _loading = false;
    }

    private async Task Stop(Guid id, long version)
    {
        _loading = true;

        var request = new FlowtimeStopRequest
        {
            FlowtimeId = id,
            FlowtimeVersion = version
        };


        var _ = await Mediator.Send<FlowtimeStopResponse>(request);

        await Refresh();

        _loading = false;
    }

    private async Task Archive()
    {
        _loading = true;

        foreach (var item in _selectedItems)
        {
            var request = new FlowtimeArchivingRequest
            {
                FlowtimeId = item.Id,
                FlowtimeVersion = item.Version
            };

            var _ = await Mediator.Send<FlowtimeArchivingResponse>(request);
        }

        await Refresh();

        _loading = false;
    }
}
