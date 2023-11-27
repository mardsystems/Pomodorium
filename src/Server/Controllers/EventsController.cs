using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.DomainModel.Storage;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<EventsController> _logger;

    public EventsController(
        IMediator mediator,
        ILogger<EventsController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpGet("", Name = "GetEvents")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventsResponse))]
    public async Task<GetEventsResponse> GetEvents([FromQuery] GetEventsRequest request)
    {
        var response = await _mediator.Send<GetEventsResponse>(request);

        return response;
    }
}
