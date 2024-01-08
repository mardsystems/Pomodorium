using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using TaskManagement.Features.TaskSynchronizer;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class TaskSynchronizerController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskSynchronizerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("TaskSyncFromTfs")]
    public async Task<TaskSyncFromTfsResponse> PostTaskSyncFromTfs(TaskSyncFromTfsRequest request)
    {
        var response = await _mediator.Send<TaskSyncFromTfsResponse>(request);

        return response;
    }

    [HttpPost("TaskSyncFromTrello")]
    public async Task<TaskSyncFromTrelloResponse> PostTaskSyncFromTrello(TaskSyncFromTrelloRequest request)
    {
        var response = await _mediator.Send<TaskSyncFromTrelloResponse>(request);

        return response;
    }
}
