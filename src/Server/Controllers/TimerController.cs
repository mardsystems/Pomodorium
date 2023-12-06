using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Pomodorium.Features.Timer;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class TimerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<TimerController> _logger;

    public TimerController(IMediator mediator, ILogger<TimerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpPost("CheckTimer")]
    public async Task<CheckTimerResponse> CheckTimer(CheckTimerRequest request)
    {
        var response = await _mediator.Send<CheckTimerResponse>(request);

        return response;
    }
}
