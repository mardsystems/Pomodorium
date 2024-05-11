using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Pomodorium.Features.PomodoroTimer;
using MediatR;

namespace Pomodorium.Pages.Pomos;

public partial class Details
{
    [Inject]
    private IMediator Mediator { get; set; }

    [Parameter]
    public string Id { get; set; }

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
            var request = new PomodoroDetailsRequest { Id = Guid.Parse(Id) };

            var response = await Mediator.Send<PomodoroDetailsResponse>(request);

            Model = new DetailsViewModel(
                response.PomodoroDetails.Id,
                response.PomodoroDetails.Task,
                response.PomodoroDetails.Timer,
                response.PomodoroDetails.StartDateTime,
                response.PomodoroDetails.State,
                response.PomodoroDetails.Version);

            Model.CountdownChanges.Subscribe(x =>
            {
                StateHasChanged();
            });
        }
    }

    private async Task Check()
    {
        var request = new PomodoroCheckingRequest
        {
            Id = Guid.Parse(Id),
            Version = Model.Version
        };

        var _ = await Mediator.Send<PomodoroCheckingResponse>(request);

        Navigation.NavigateTo("pomos");
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
            var request = new PomodoroCreationRequest
            {
                Task = Model.Task,
                Timer = Model.Timer.Value
            };

            var _ = await Mediator.Send<PomodoroCreationResponse>(request);
        }
        else
        {
            var request = new PomodoroTaskRefinementRequest
            {
                Id = Guid.Parse(Id),
                Task = Model.Task,
                Version = Model.Version
            };

            var _ = await Mediator.Send<PomodoroTaskRefinementResponse>(request);
        }

        Navigation.NavigateTo("pomos");
    }

    private async Task Archive()
    {
        if (Id == null || Id == "new")
        {
            return;
        }

        var request = new PomodoroArchivingRequest
        {
            Id = Guid.Parse(Id),
            Version = Model.Version
        };

        var _ = await Mediator.Send<PomodoroArchivingResponse>(request);

        Navigation.NavigateTo("pomos");
    }
}
