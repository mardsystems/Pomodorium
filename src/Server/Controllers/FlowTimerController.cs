using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pomodorium.Features.FlowTimer;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlowTimerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<FlowTimerController> _logger;

    public FlowTimerController(
        IMediator mediator,
        ILogger<FlowTimerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpGet("Flows", Name = "GetFlows")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetFlowsResponse))]
    public async Task<GetFlowsResponse> GetFlows([FromQuery] GetFlowsRequest request)
    {
        var response = await _mediator.Send<GetFlowsResponse>(request);

        return response;
    }

    [HttpPost("Flows/{id}/Stop", Name = "StopFlowtime")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(StopFlowtimeResponse))]
    public async Task<StopFlowtimeResponse> StopFlowtime(Guid id, StopFlowtimeRequest request)
    {
        request.Id = id;

        var response = await _mediator.Send<StopFlowtimeResponse>(request);

        return response;
    }
}