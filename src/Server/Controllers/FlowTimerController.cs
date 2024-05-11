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

    public FlowTimerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("FlowtimeQuery")]
    public async Task<FlowtimeQueryResponse> GetFlowtimeQuery([FromQuery] FlowtimeQueryRequest request)
    {
        var response = await _mediator.Send<FlowtimeQueryResponse>(request);

        return response;
    }

    [HttpGet("FlowtimeDetails")]
    public async Task<FlowtimeDetailsResponse> GetFlowtimeDetails([FromQuery] FlowtimeDetailsRequest request)
    {
        var response = await _mediator.Send<FlowtimeDetailsResponse>(request);

        return response;
    }

    [HttpPost("FlowtimeCreation")]
    public async Task<FlowtimeCreationResponse> PostFlowtimeCreation(FlowtimeCreationRequest request)
    {
        var response = await _mediator.Send<FlowtimeCreationResponse>(request);

        return response;
    }

    [HttpPost("FlowtimeCreationFromTask")]
    public async Task<FlowtimeCreationFromTaskResponse> PostFlowtimeCreationFromTask(FlowtimeCreationFromTaskRequest request)
    {
        var response = await _mediator.Send<FlowtimeCreationFromTaskResponse>(request);

        return response;
    }

    [HttpPost("FlowtimeStart")]
    public async Task<FlowtimeStartResponse> PostFlowtimeStart(FlowtimeStartRequest request)
    {
        var response = await _mediator.Send<FlowtimeStartResponse>(request);

        return response;
    }

    [HttpPost("FlowtimeStartFromTask")]
    public async Task<FlowtimeStartFromTaskResponse> PostFlowtimeStartFromTask(FlowtimeStartFromTaskRequest request)
    {
        var response = await _mediator.Send<FlowtimeStartFromTaskResponse>(request);

        return response;
    }

    [HttpPost("FlowtimeInterruption")]
    public async Task<FlowtimeInterruptionResponse> PostFlowtimeInterruption(FlowtimeInterruptionRequest request)
    {
        var response = await _mediator.Send<FlowtimeInterruptionResponse>(request);

        return response;
    }

    [HttpPost("FlowtimeStop")]
    public async Task<FlowtimeStopResponse> PostFlowtimeStop(FlowtimeStopRequest request)
    {
        var response = await _mediator.Send<FlowtimeStopResponse>(request);

        return response;
    }

    [HttpPost("FlowtimeArchiving")]
    public async Task<FlowtimeArchivingResponse> PostFlowtimeArchiving(FlowtimeArchivingRequest request)
    {
        var response = await _mediator.Send<FlowtimeArchivingResponse>(request);

        return response;
    }
}
