using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Pomodorium.Features.FlowTimer;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class FlowTimerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<FlowTimerController> _logger;

    public FlowTimerController(IMediator mediator, ILogger<FlowTimerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpPost("GetFlows")]
    public async Task<FlowtimeQueryResponse> GetFlows(FlowtimeQueryRequest request)
    {
        var response = await _mediator.Send<FlowtimeQueryResponse>(request);

        return response;
    }

    [HttpPost("GetFlowtime")]
    public async Task<FlowtimeDetailsResponse> GetFlowtime(FlowtimeDetailsRequest request)
    {
        var response = await _mediator.Send<FlowtimeDetailsResponse>(request);

        return response;
    }

    [HttpPost("CreateFlowtime")]
    public async Task<FlowtimeCreationResponse> CreateFlowtime(FlowtimeCreationRequest request)
    {
        var response = await _mediator.Send<FlowtimeCreationResponse>(request);

        return response;
    }

    [HttpPost("CreateFlowtimeFromTask")]
    public async Task<FlowtimeCreationFromTaskResponse> CreateFlowtimeFromTask(FlowtimeCreationFromTaskRequest request)
    {
        var response = await _mediator.Send<FlowtimeCreationFromTaskResponse>(request);

        return response;
    }

    [HttpPost("StartFlowtime")]
    public async Task<FlowtimeStartResponse> StartFlowtime(FlowtimeStartRequest request)
    {
        var response = await _mediator.Send<FlowtimeStartResponse>(request);

        return response;
    }

    [HttpPost("StartFlowtimeFromTask")]
    public async Task<FlowtimeStartFromTaskResponse> StartFlowtimeFromTask(FlowtimeStartFromTaskRequest request)
    {
        var response = await _mediator.Send<FlowtimeStartFromTaskResponse>(request);

        return response;
    }

    [HttpPost("InterruptFlowtime")]
    public async Task<FlowtimeInterruptionResponse> InterruptFlowtime(FlowtimeInterruptionRequest request)
    {
        var response = await _mediator.Send<FlowtimeInterruptionResponse>(request);

        return response;
    }

    [HttpPost("StopFlowtime")]
    public async Task<FlowtimeStopResponse> StopFlowtime(FlowtimeStopRequest request)
    {
        var response = await _mediator.Send<FlowtimeStopResponse>(request);

        return response;
    }

    [HttpPost("ArchiveFlowtime")]
    public async Task<FlowtimeArchivingResponse> ArchiveFlowtime(FlowtimeArchivingRequest request)
    {
        var response = await _mediator.Send<FlowtimeArchivingResponse>(request);

        return response;
    }
}
