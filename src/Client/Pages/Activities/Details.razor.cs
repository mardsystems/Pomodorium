using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskManagement.Features.ActivityManager;
using MediatR;

namespace Pomodorium.Pages.Activities;

public partial class Details
{
    [Inject]
    private IMediator Mediator { get; set; }

    [Parameter]
    public string Id { get; set; }

    public DetailsViewModel Model { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; }

    private bool _success;

    protected override async Task OnInitializedAsync()
    {
        if (Id == null || Id == "new")
        {
            Model = new DetailsViewModel();
        }
        else
        {
            var request = new ActivityDetailsRequest { Id = Guid.Parse(Id) };

            var response = await Mediator.Send<ActivityDetailsResponse>(request);

            Model = new DetailsViewModel(
                response.ActivityDetails.Id,
                response.ActivityDetails.Name,
                response.ActivityDetails.StartDateTime,
                response.ActivityDetails.StopDateTime,
                response.ActivityDetails.State,
                response.ActivityDetails.Duration,
                response.ActivityDetails.Description,
                response.ActivityDetails.Version);
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
            var request = new ActivityCreationRequest
            {
                Name = Model.Name,
                StartDateTime = Model.GetStartDateTime(),
                StopDateTime = Model.GetStopDateTime(),
                Description = Model.Description
            };

            var _ = await Mediator.Send<ActivityCreationResponse>(request);
        }
        else
        {
            var request = new ActivityUpdatingRequest
            {
                Id = Guid.Parse(Id),
                Name = Model.Name,
                StartDateTime = Model.GetStartDateTime(),
                StopDateTime = Model.GetStopDateTime(),
                Description = Model.Description,
                Version = Model.Version
            };

            var _ = await Mediator.Send<ActivityUpdatingResponse>(request);
        }

        Navigation.NavigateTo("activities");
    }

    private async Task Delete()
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning",
            "Deleting can not be undone!",
            yesText: "Delete!", cancelText: "Cancel");

        if (result == true)
        {
            var request = new ActivityDeletionRequest
            {
                Id = Guid.Parse(Id),
                Version = Model.Version
            };

            var _ = await Mediator.Send<ActivityDeletionResponse>(request);

            Navigation.NavigateTo("activities");
        }
    }
}
