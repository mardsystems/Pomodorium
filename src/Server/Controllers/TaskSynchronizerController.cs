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
    public async Task<SyncTasksFromTfsResponse> SyncTasksFromTfs(SyncTasksFromTfsRequest request)
    {
        var response = await _mediator.Send<SyncTasksFromTfsResponse>(request);

        return response;
    }

    [HttpPost("SyncTasksFromTrello")]
    public async Task<SyncTasksFromTrelloResponse> SyncTasksFromTrello(SyncTasksFromTrelloRequest request)
    {
        var response = await _mediator.Send<SyncTasksFromTrelloResponse>(request);

        return response;
    }
}
