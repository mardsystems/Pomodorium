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

    public TaskManagerController(IMediator mediator, ILogger<TaskManagerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpPost("GetTasks")]
    public async Task<GetTasksResponse> GetTasks(GetTasksRequest request)
    {
        var response = await _mediator.Send<GetTasksResponse>(request);

        return response;
    }

    [HttpPost("CreateTask")]
    public async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request)
    {
        var response = await _mediator.Send<CreateTaskResponse>(request);

        return response;
    }

    [HttpPost("GetTask")]
    public async Task<GetTaskResponse> GetTask(GetTaskRequest request)
    {
        var response = await _mediator.Send<GetTaskResponse>(request);

        return response;
    }

    [HttpPost("ChangeTaskDescription")]
    public async Task<ChangeTaskDescriptionResponse> ChangeTaskDescription(ChangeTaskDescriptionRequest request)
    {
        var response = await _mediator.Send<ChangeTaskDescriptionResponse>(request);

        return response;
    }

    [HttpPost("FocusWithFlowtime")]
    public async Task<StartFlowtimeFromTaskResponse> FocusWithFlowtime(StartFlowtimeFromTaskRequest request)
    {
        var response = await _mediator.Send<StartFlowtimeFromTaskResponse>(request);

        return response;
    }

    [HttpPost("ArchiveTask")]
    public async Task<ArchiveTaskResponse> ArchiveTask(ArchiveTaskRequest request)
    {
        var response = await _mediator.Send<ArchiveTaskResponse>(request);

        return response;
    }
}
