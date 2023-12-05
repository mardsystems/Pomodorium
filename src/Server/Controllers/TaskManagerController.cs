using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Pomodorium.Features.FlowTimer;
using Pomodorium.Features.TaskManager;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
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

    [HttpGet("Tasks", Name = "GetTasks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTasksResponse))]
    public async Task<GetTasksResponse> GetTasks([FromQuery] GetTasksRequest request)
    {
        var response = await _mediator.Send<GetTasksResponse>(request);

        return response;
    }

    [HttpPost("Tasks/Create", Name = "CreateTask")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(CreateTaskResponse))]
    public async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request)
    {
        var response = await _mediator.Send<CreateTaskResponse>(request);

        return response;
    }

    [HttpGet("Tasks/{id}", Name = "GetTask")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTaskResponse))]
    public async Task<GetTaskResponse> GetTask(Guid id)
    {
        var request = new GetTaskRequest { TaskId = id };

        var response = await _mediator.Send<GetTaskResponse>(request);

        return response;
    }

    [HttpPost("Tasks/{id}/Description/Change", Name = "ChangeTaskDescription")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChangeTaskDescriptionResponse))]
    public async Task<ChangeTaskDescriptionResponse> ChangeTaskDescription(Guid id, ChangeTaskDescriptionRequest request)
    {
        request.TaskId = id;

        var response = await _mediator.Send<ChangeTaskDescriptionResponse>(request);

        return response;
    }

    [HttpPost("Tasks/{id}/FocusWithFlowtime", Name = "FocusWithFlowtime")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StartFlowtimeFromTaskResponse))]
    public async Task<StartFlowtimeFromTaskResponse> FocusWithFlowtime(Guid id, StartFlowtimeFromTaskRequest request)
    {
        request.TaskId = id;

        var response = await _mediator.Send<StartFlowtimeFromTaskResponse>(request);

        return response;
    }

    [HttpPost("Tasks/{id}/Archive", Name = "ArchiveTask")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArchiveTaskResponse))]
    public async Task<ArchiveTaskResponse> ArchiveTask(Guid id, long version)
    {
        var request = new ArchiveTaskRequest { TaskId = id, TaskVersion = version };

        var response = await _mediator.Send<ArchiveTaskResponse>(request);

        return response;
    }
}
