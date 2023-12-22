using MediatR;
using Microsoft.AspNetCore.Components;
using Pomodorium.Features.FlowTimer;
using Pomodorium.Features.TaskManager;
using Pomodorium.Features.TaskSynchronizer;

namespace Pomodorium.Pages.Tasks;

public partial class Index
{
    [Inject]
    private IMediator Mediator { get; set; }

    //[Inject] EventHubClient EventHubClient

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

        var request = new TaskQueryRequest { };

        var response = await Mediator.Send<TaskQueryResponse>(request);

        _model.Repopulate(response.TaskQueryItems);

        _loading = false;
    }

    private async Task Add()
    {
        NavigationManager.NavigateTo($"/tasks/new");

        await Task.CompletedTask;
    }

    private async Task SyncTasksFromTFS()
    {
        _loading = true;

        var request = new TaskSyncFromTfsRequest
        {

        };

        await Mediator.Send<TaskSyncFromTfsResponse>(request);

        _loading = false;
    }

    private async Task SyncTasksFromTrello()
    {
        _loading = true;

        var request = new TaskSyncFromTrelloRequest
        {

        };

        await Mediator.Send<TaskSyncFromTrelloResponse>(request);

        _loading = false;
    }

    private async Task Details(Guid id)
    {
        NavigationManager.NavigateTo($"/tasks/{id}");

        await Task.CompletedTask;
    }

    private async Task FocusWithFlowtime(Guid taskId)
    {
        _loading = true;

        var request = new FlowtimeStartFromTaskRequest
        {
            TaskId = taskId
        };

        var _ = await Mediator.Send<FlowtimeStartFromTaskResponse>(request);

        await Refresh();

        _loading = false;
    }

    private async Task StopFlowtime(Guid id)
    {
        _loading = true;

        var request = new FlowtimeStopRequest
        {
            FlowtimeId = id
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
            var request = new TaskArchivingRequest
            {
                TaskId = item.Id,
                TaskVersion = item.Version
            };

            var _ = await Mediator.Send<TaskArchivingResponse>(request);
        }

        await Refresh();

        _loading = false;
    }
}
