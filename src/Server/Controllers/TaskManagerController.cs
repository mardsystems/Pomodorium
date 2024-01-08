using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using FlowtimeTechnique.Features.FlowTimer;
using TaskManagement.Features.TaskManager;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class TaskManagerController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskManagerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("TaskQuery")]
    public async Task<TaskQueryResponse> GetTaskQuery([FromQuery] TaskQueryRequest request)
    {
        var response = await _mediator.Send<TaskQueryResponse>(request);

        return response;
    }

    [HttpGet("TaskDetails")]
    public async Task<TaskDetailsResponse> GetTaskDetails([FromQuery] TaskDetailsRequest request)
    {
        var response = await _mediator.Send<TaskDetailsResponse>(request);

        return response;
    }

    [HttpPost("TaskRegistration")]
    public async Task<TaskRegistrationResponse> PostTaskRegistration(TaskRegistrationRequest request)
    {
        var response = await _mediator.Send<TaskRegistrationResponse>(request);

        return response;
    }

    [HttpPost("TaskDescriptionChange")]
    public async Task<TaskDescriptionChangeResponse> PostTaskDescriptionChange(TaskDescriptionChangeRequest request)
    {
        var response = await _mediator.Send<TaskDescriptionChangeResponse>(request);

        return response;
    }

    [HttpPost("TaskArchiving")]
    public async Task<TaskArchivingResponse> PostTaskArchiving(TaskArchivingRequest request)
    {
        var response = await _mediator.Send<TaskArchivingResponse>(request);

        return response;
    }
}
