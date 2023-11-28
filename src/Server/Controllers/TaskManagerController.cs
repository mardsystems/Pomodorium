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

    [HttpPost("", Name = "PostSyncTasksWithTFSRequest")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SyncTasksWithTFSResponse))]
    public async Task<SyncTasksWithTFSResponse> PostSyncTasksWithTFSRequest(SyncTasksWithTFSRequest request)
    {
        var response = await _mediator.Send<SyncTasksWithTFSResponse>(request);

        return response;
    }
}
