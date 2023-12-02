using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pomodorium.Features.Timer;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<TimerController> _logger;

    public TimerController(
        IMediator mediator,
        ILogger<TimerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpPost("Check", Name = "PostTimerCheck")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(PostTimerCheckResponse))]
    public async Task<PostTimerCheckResponse> PostTimerCheck(PostTimerCheckRequest request)
    {
        var response = await _mediator.Send<PostTimerCheckResponse>(request);

        return response;
    }
}
