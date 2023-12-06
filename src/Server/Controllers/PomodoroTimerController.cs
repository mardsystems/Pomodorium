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
    public async Task<GetPomosResponse> GetPomos(GetPomosRequest request)
    {
        var response = await _mediator.Send<GetPomosResponse>(request);

        return response;
    }

    [HttpPost("GetPomodoro")]
    public async Task<GetPomodoroResponse> GetPomodoro(GetPomodoroRequest request)
    {
        var response = await _mediator.Send<GetPomodoroResponse>(request);

        return response;
    }

    [HttpPost("CreatePomodoro")]
    public async Task<CreatePomodoroResponse> CreatePomodoro(CreatePomodoroRequest request)
    {
        var response = await _mediator.Send<CreatePomodoroResponse>(request);

        return response;
    }

    [HttpPost("CheckPomodoro")]
    public async Task<CheckPomodoroResponse> CheckPomodoro(CheckPomodoroRequest request)
    {
        var response = await _mediator.Send<CheckPomodoroResponse>(request);

        return response;
    }

    [HttpPost("RefinePomodoroTask")]
    public async Task<RefinePomodoroTaskResponse> RefinePomodoroTask(RefinePomodoroTaskRequest request)
    {
        var response = await _mediator.Send<RefinePomodoroTaskResponse>(request);

        return response;
    }

    [HttpPost("ArchivePomodoro")]
    public async Task<ArchivePomodoroResponse> ArchivePomodoro(ArchivePomodoroRequest request)
    {
        var response = await _mediator.Send<ArchivePomodoroResponse>(request);

        return response;
    }
}
