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

    public TimerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CheckTimer")]
    public async Task<CheckTimerResponse> CheckTimer(CheckTimerRequest request)
    {
        var response = await _mediator.Send<CheckTimerResponse>(request);

        return response;
    }
}
