using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimersController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<TimersController> _logger;

    public TimersController(
        IMediator mediator,
        ILogger<TimersController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpGet("", Name = "GetTimers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTimersResponse))]
    public async Task<GetTimersResponse> GetTimers([FromQuery] GetTimersRequest request)
    {
        var response = await _mediator.Send<GetTimersResponse>(request);

        return response;
    }

    [HttpPost("", Name = "PostPomodoro")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostPomodoroResponse))]
    public async Task<PostPomodoroResponse> PostPomodoro(PostPomodoroRequest request)
    {
        var response = await _mediator.Send<PostPomodoroResponse>(request);

        return response;
    }

    [HttpGet("{id}", Name = "GetPomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPomodoroResponse))]
    public async Task<GetPomodoroResponse> GetPomodoro(Guid id)
    {
        var request = new GetPomodoroRequest { Id = id };

        var response = await _mediator.Send<GetPomodoroResponse>(request);

        return response;
    }

    [HttpPut("{id}", Name = "PutPomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PutPomodoroResponse))]
    public async Task<PutPomodoroResponse> PutPomodoro(Guid id, PutPomodoroRequest request)
    {
        request.Id = id;

        var response = await _mediator.Send<PutPomodoroResponse>(request);

        return response;
    }

    [HttpDelete("{id}", Name = "DeletePomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeletePomodoroResponse))]
    public async Task<DeletePomodoroResponse> DeletePomodoro(Guid id, long version)
    {
        var request = new DeletePomodoroRequest { Id = id, Version = version };

        var response = await _mediator.Send<DeletePomodoroResponse>(request);

        return response;
    }
}
