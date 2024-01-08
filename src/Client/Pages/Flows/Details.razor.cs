using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using FlowtimeTechnique.Features.FlowTimer;
using TaskManagement.Features.TaskManager;

namespace Pomodorium.Pages.Flows;

public partial class Details
{
    [Inject]
    private IMediator Mediator { get; set; }

    [Parameter]
    public string Id { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public Guid? TaskId { get; set; }

    public DetailsViewModel Model { get; set; } = default!;

    private bool _success;

    protected override async Task OnInitializedAsync()
    {
        if (Id == null || Id == "new")
        {
            Model = new DetailsViewModel();
        }
        else if (Id == "continuation")
        {
            if (TaskId == null)
            {
                throw new InvalidOperationException();
            }

            var request = new TaskDetailsRequest(TaskId.Value);

            var response = await Mediator.Send<TaskDetailsResponse>(request);

            Model = new DetailsViewModel(
                response.TaskDetails.Id,
                response.TaskDetails.Description,
                response.TaskDetails.Version
            );
        }
        else
        {
            var flowtimeId = Guid.Parse(Id);

            var request = new FlowtimeDetailsRequest(flowtimeId);

            var response = await Mediator.Send<FlowtimeDetailsResponse>(request);

            Model = new DetailsViewModel(
                response.FlowtimeDetails.Id,
                response.FlowtimeDetails.CreationDate,
                response.FlowtimeDetails.TaskId,
                response.FlowtimeDetails.TaskDescription,
                response.FlowtimeDetails.TaskVersion,
                response.FlowtimeDetails.StartDateTime,
                response.FlowtimeDetails.StopDateTime,
                response.FlowtimeDetails.Interrupted,
                response.FlowtimeDetails.Worktime,
                response.FlowtimeDetails.Breaktime,
                response.FlowtimeDetails.State,
                response.FlowtimeDetails.Version);

            Model.BreakCountdownChanges.Subscribe(x =>
            {
                StateHasChanged();
            });
        }
    }

    private async Task OnValidSubmit(EditContext context)
    {
        await Save();

        _success = true;

        StateHasChanged();
    }

    private async Task Start()
    {
        var request = new FlowtimeStartRequest
        {
            FlowtimeId = Model.Id,
            FlowtimeVersion = Model.Version
        };

        var _ = await Mediator.Send<FlowtimeStartResponse>(request);

        Navigation.NavigateTo("flows");
    }

    private async Task Stop()
    {
        var request = new FlowtimeStopRequest
        {
            FlowtimeId = Model.Id,
            FlowtimeVersion = Model.Version
        };

        var _ = await Mediator.Send<FlowtimeStopResponse>(request);

        Navigation.NavigateTo("flows");
    }

    private async Task Save()
    {
        if (Id == null || Id == "new")
        {
            var request = new FlowtimeCreationRequest
            {
                TaskDescription = Model.TaskDescription
            };

            var _ = await Mediator.Send<FlowtimeCreationResponse>(request);
        }
        else if (Id == "continuation")
        {
            var request = new FlowtimeCreationFromTaskRequest
            {
                TaskId = Model.TaskId,
                TaskDescription = Model.TaskDescription,
                TaskVersion = Model.TaskVersion
            };

            var _ = await Mediator.Send<FlowtimeCreationFromTaskResponse>(request);
        }
        else
        {
            var request = new TaskDescriptionChangeRequest
            {
                TaskId = Model.TaskId,
                Description = Model.TaskDescription,
                TaskVersion = Model.TaskVersion
            };

            var _ = await Mediator.Send<TaskDescriptionChangeResponse>(request);
        }

        Navigation.NavigateTo("flows");
    }

    private async Task Archive()
    {
        if (Id == null || Id == "new")
        {
            return;
        }

        var flowtimeId = Guid.Parse(Id);

        var request = new FlowtimeArchivingRequest
        {
            FlowtimeId = flowtimeId,
            FlowtimeVersion = Model.Version
        };

        var _ = await Mediator.Send<FlowtimeArchivingResponse>(request);

        Navigation.NavigateTo("flows");
    }
}
