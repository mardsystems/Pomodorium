using MediatR;
using Microsoft.AspNetCore.Components;
using Pomodorium.Features.Maintenance;

namespace Pomodorium.Pages;

public partial class Index
{
    [Inject]
    private IMediator Mediator { get; set; }

    private async Task RebuildIndex()
    {
        var request = new IndexRebuildRequest();

        var _ = await Mediator.Send<IndexRebuildResponse>(request);
    }
}
