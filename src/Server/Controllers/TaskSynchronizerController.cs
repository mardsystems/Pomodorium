using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Pomodorium.Features.TaskSynchronizer;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class TaskSynchronizerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<TaskSynchronizerController> _logger;

    public TaskSynchronizerController(IMediator mediator, ILogger<TaskSynchronizerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpPost("SyncTasksFromTfs")]
    public async Task<TaskSyncFromTfsResponse> SyncTasksFromTfs(TaskSyncFromTfsRequest request)
    {
        var response = await _mediator.Send<TaskSyncFromTfsResponse>(request);

        return response;
    }

    [HttpPost("SyncTasksFromTrello")]
    public async Task<TaskSyncFromTrelloResponse> SyncTasksFromTrello(TaskSyncFromTrelloRequest request)
    {
        var response = await _mediator.Send<TaskSyncFromTrelloResponse>(request);

        return response;
    }
}
