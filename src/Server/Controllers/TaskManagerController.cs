using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pomodorium.Features.TaskManager;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskManagerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<TaskManagerController> _logger;

    public TaskManagerController(
        IMediator mediator,
        ILogger<TaskManagerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpPost("syncTasksWithTFS", Name = "PostSyncTasksWithTFS")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SyncTasksWithTFSResponse))]
    public async Task<SyncTasksWithTFSResponse> SyncTasksWithTFS(SyncTasksWithTFSRequest request)
    {
        var response = await _mediator.Send<SyncTasksWithTFSResponse>(request);

        return response;
    }

    [HttpPost("syncTasksWithTrello", Name = "SyncTasksWithTrelloRequest")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SyncTasksWithTrelloResponse))]
    public async Task<SyncTasksWithTrelloResponse> SyncTasksWithTrello(SyncTasksWithTrelloRequest request)
    {
        var response = await _mediator.Send<SyncTasksWithTrelloResponse>(request);

        return response;
    }
}
