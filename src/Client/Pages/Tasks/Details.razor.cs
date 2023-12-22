using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Pomodorium.Features.TaskManager;
using MediatR;

namespace Pomodorium.Pages.Tasks;

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
        else
        {
            var taskId = Guid.Parse(Id);

            var request = new TaskDetailsRequest(taskId);

            var response = await Mediator.Send<TaskDetailsResponse>(request);

            Model = new DetailsViewModel(
                response.TaskDetails.Id,
                response.TaskDetails.CreationDate,
                response.TaskDetails.Description,
                response.TaskDetails.Version);
        }
    }

    private async Task OnValidSubmit(EditContext context)
    {
        await Save();

        _success = true;

        StateHasChanged();
    }

    private async Task Save()
    {
        if (Id == null || Id == "new")
        {
            var request = new TaskRegistrationRequest
            {
                Description = Model.Description
            };

            var _ = await Mediator.Send<TaskRegistrationResponse>(request);
        }
        else
        {
            var request = new TaskDescriptionChangeRequest
            {
                TaskId = Model.Id,
                Description = Model.Description,
                TaskVersion = Model.Version
            };

            var _ = await Mediator.Send<TaskDescriptionChangeResponse>(request);
        }

        Navigation.NavigateTo("tasks");
    }

    private async Task Archive()
    {
        if (Id == null || Id == "new")
        {
            return;
        }

        var taskId = Guid.Parse(Id);

        var request = new TaskArchivingRequest
        {
            TaskId = taskId,
            TaskVersion = Model.Version
        };

        var _ = await Mediator.Send<TaskArchivingResponse>(request);

        Navigation.NavigateTo("tasks");
    }
}
