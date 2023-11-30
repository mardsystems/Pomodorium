using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pomodorium.Features.TaskSynchronizer;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskSynchronizerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<TaskSynchronizerController> _logger;

    public TaskSynchronizerController(
        IMediator mediator,
        ILogger<TaskSynchronizerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpPost("SyncTasksFromTfs", Name = "PostSyncTasksFromTfs")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SyncTasksFromTfsResponse))]
    public async Task<SyncTasksFromTfsResponse> SyncTasksFromTfs(SyncTasksFromTfsRequest request)
    {
        var response = await _mediator.Send<SyncTasksFromTfsResponse>(request);

        return response;
    }

    [HttpPost("SyncTasksFromTrello", Name = "PostSyncTasksFromTrello")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SyncTasksFromTrelloResponse))]
    public async Task<SyncTasksFromTrelloResponse> SyncTasksFromTrello(SyncTasksFromTrelloRequest request)
    {
        var response = await _mediator.Send<SyncTasksFromTrelloResponse>(request);

        return response;
    }
}
