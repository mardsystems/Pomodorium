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
    public async Task<TaskQueryResponse> GetTasks(TaskQueryRequest request)
    {
        var response = await _mediator.Send<TaskQueryResponse>(request);

        return response;
    }

    [HttpPost("CreateTask")]
    public async Task<TaskRegistrationResponse> CreateTask(TaskRegistrationRequest request)
    {
        var response = await _mediator.Send<TaskRegistrationResponse>(request);

        return response;
    }

    [HttpPost("GetTask")]
    public async Task<TaskDetailsResponse> GetTask(TaskDetailsRequest request)
    {
        var response = await _mediator.Send<TaskDetailsResponse>(request);

        return response;
    }

    [HttpPost("ChangeTaskDescription")]
    public async Task<TaskDescriptionChangeResponse> ChangeTaskDescription(TaskDescriptionChangeRequest request)
    {
        var response = await _mediator.Send<TaskDescriptionChangeResponse>(request);

        return response;
    }

    [HttpPost("FocusWithFlowtime")]
    public async Task<FlowtimeStartFromTaskResponse> FocusWithFlowtime(FlowtimeStartFromTaskRequest request)
    {
        var response = await _mediator.Send<FlowtimeStartFromTaskResponse>(request);

        return response;
    }

    [HttpPost("ArchiveTask")]
    public async Task<TaskArchiveResponse> ArchiveTask(TaskArchiveRequest request)
    {
        var response = await _mediator.Send<TaskArchiveResponse>(request);

        return response;
    }
}
