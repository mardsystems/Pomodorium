using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using PomodoroTechnique.Features.PomodoroTimer;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class PomodoroTimerController : ControllerBase
{
    private readonly IMediator _mediator;

    public PomodoroTimerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("PomodoroQuery")]
    public async Task<PomodoroQueryResponse> GetPomodoroQuery([FromQuery] PomodoroQueryRequest request)
    {
        var response = await _mediator.Send<PomodoroQueryResponse>(request);

        return response;
    }

    [HttpPost("PomodoroDetails")]
    public async Task<PomodoroDetailsResponse> GetPomodoroDetails([FromQuery] PomodoroDetailsRequest request)
    {
        var response = await _mediator.Send<PomodoroDetailsResponse>(request);

        return response;
    }

    [HttpPost("PomodoroCreation")]
    public async Task<PomodoroCreationResponse> PostPomodoroCreation(PomodoroCreationRequest request)
    {
        var response = await _mediator.Send<PomodoroCreationResponse>(request);

        return response;
    }

    [HttpPost("PomodoroChecking")]
    public async Task<PomodoroCheckingResponse> PostPomodoroChecking(PomodoroCheckingRequest request)
    {
        var response = await _mediator.Send<PomodoroCheckingResponse>(request);

        return response;
    }

    [HttpPost("PomodoroTaskRefinement")]
    public async Task<PomodoroTaskRefinementResponse> PostPomodoroTaskRefinement(PomodoroTaskRefinementRequest request)
    {
        var response = await _mediator.Send<PomodoroTaskRefinementResponse>(request);

        return response;
    }

    [HttpPost("PomodoroArchiving")]
    public async Task<PomodoroArchivingResponse> PostPomodoroArchiving(PomodoroArchivingRequest request)
    {
        var response = await _mediator.Send<PomodoroArchivingResponse>(request);

        return response;
    }
}
