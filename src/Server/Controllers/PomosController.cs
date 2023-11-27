using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pomodorium.TimeManagement.PomodoroTimer;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PomosController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<PomosController> _logger;

    public PomosController(
        IMediator mediator,
        ILogger<PomosController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpGet("", Name = "GetPomos")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPomosResponse))]
    public async Task<GetPomosResponse> GetPomos([FromQuery] GetPomosRequest request)
    {
        var response = await _mediator.Send<GetPomosResponse>(request);

        return response;
    }

    [HttpPost("", Name = "PostPomodoro")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePomodoroResponse))]
    public async Task<CreatePomodoroResponse> PostPomodoro(CreatePomodoroRequest request)
    {
        var response = await _mediator.Send<CreatePomodoroResponse>(request);

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefinePomodoroTaskResponse))]
    public async Task<RefinePomodoroTaskResponse> PutPomodoro(Guid id, RefinePomodoroTaskRequest request)
    {
        request.Id = id;

        var response = await _mediator.Send<RefinePomodoroTaskResponse>(request);

        return response;
    }

    [HttpDelete("{id}", Name = "DeletePomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArchivePomodoroResponse))]
    public async Task<ArchivePomodoroResponse> DeletePomodoro(Guid id, long version)
    {
        var request = new ArchivePomodoroRequest { Id = id, Version = version };

        var response = await _mediator.Send<ArchivePomodoroResponse>(request);

        return response;
    }
}
