using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pomodorium.Features.PomodoroTimer;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PomodoroTimerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<PomodoroTimerController> _logger;

    public PomodoroTimerController(
        IMediator mediator,
        ILogger<PomodoroTimerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpGet("Pomos", Name = "GetPomos")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPomosResponse))]
    public async Task<GetPomosResponse> GetPomos([FromQuery] GetPomosRequest request)
    {
        var response = await _mediator.Send<GetPomosResponse>(request);

        return response;
    }

    [HttpPost("Pomos", Name = "PostPomodoro")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePomodoroResponse))]
    public async Task<CreatePomodoroResponse> PostPomodoro(CreatePomodoroRequest request)
    {
        var response = await _mediator.Send<CreatePomodoroResponse>(request);

        return response;
    }

    [HttpGet("Pomos/{id}", Name = "GetPomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPomodoroResponse))]
    public async Task<GetPomodoroResponse> GetPomodoro(Guid id)
    {
        var request = new GetPomodoroRequest { Id = id };

        var response = await _mediator.Send<GetPomodoroResponse>(request);

        return response;
    }

    [HttpPut("Pomos/{id}", Name = "PutPomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefinePomodoroTaskResponse))]
    public async Task<RefinePomodoroTaskResponse> PutPomodoro(Guid id, RefinePomodoroTaskRequest request)
    {
        request.Id = id;

        var response = await _mediator.Send<RefinePomodoroTaskResponse>(request);

        return response;
    }

    [HttpDelete("Pomos/{id}", Name = "DeletePomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArchivePomodoroResponse))]
    public async Task<ArchivePomodoroResponse> DeletePomodoro(Guid id, long version)
    {
        var request = new ArchivePomodoroRequest { Id = id, Version = version };

        var response = await _mediator.Send<ArchivePomodoroResponse>(request);

        return response;
    }
}
