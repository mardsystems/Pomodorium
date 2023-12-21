using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Pomodorium.Features.PomodoroTimer;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class PomodoroTimerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<PomodoroTimerController> _logger;

    public PomodoroTimerController(IMediator mediator, ILogger<PomodoroTimerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpPost("GetPomos")]
    public async Task<PomodoroQueryResponse> GetPomos(PomodoroQueryRequest request)
    {
        var response = await _mediator.Send<PomodoroQueryResponse>(request);

        return response;
    }

    [HttpPost("GetPomodoro")]
    public async Task<PomodoroDetailsResponse> GetPomodoro(PomodoroDetailsRequest request)
    {
        var response = await _mediator.Send<PomodoroDetailsResponse>(request);

        return response;
    }

    [HttpPost("CreatePomodoro")]
    public async Task<PomodoroCreationResponse> CreatePomodoro(PomodoroCreationRequest request)
    {
        var response = await _mediator.Send<PomodoroCreationResponse>(request);

        return response;
    }

    [HttpPost("CheckPomodoro")]
    public async Task<PomodoroCheckingResponse> CheckPomodoro(PomodoroCheckingRequest request)
    {
        var response = await _mediator.Send<PomodoroCheckingResponse>(request);

        return response;
    }

    [HttpPost("RefinePomodoroTask")]
    public async Task<PomodoroTaskRefinementResponse> RefinePomodoroTask(PomodoroTaskRefinementRequest request)
    {
        var response = await _mediator.Send<PomodoroTaskRefinementResponse>(request);

        return response;
    }

    [HttpPost("ArchivePomodoro")]
    public async Task<PomodoroArchivingResponse> ArchivePomodoro(PomodoroArchivingRequest request)
    {
        var response = await _mediator.Send<PomodoroArchivingResponse>(request);

        return response;
    }
}
