using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PomodoriController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<PomodoriController> _logger;

    public PomodoriController(
        IMediator mediator,
        ILogger<PomodoriController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpGet("", Name = "GetPomodori")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPomodoriResponse))]
    public async Task<GetPomodoriResponse> GetPomodori([FromQuery] GetPomodoriRequest request)
    {
        var response = await _mediator.Send<GetPomodoriResponse>(request);

        return response;
    }

    [HttpPost("", Name = "PostPomodoro")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostPomodoroResponse))]
    public async Task<PostPomodoroResponse> PostPomodoro(PostPomodoroRequest request)
    {
        var response = await _mediator.Send<PostPomodoroResponse>(request);

        return response;
    }

    [HttpGet("{id:int}", Name = "GetPomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPomodoroResponse))]
    public async Task<GetPomodoroResponse> GetPomodoro(string id)
    {
        var request = new GetPomodoroRequest { Id = id };

        var response = await _mediator.Send<GetPomodoroResponse>(request);

        return response;
    }

    [HttpPut("{id:int}", Name = "PutPomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PutPomodoroResponse))]
    public async Task<PutPomodoroResponse> PutPomodoro(string id, PutPomodoroRequest request)
    {
        var response = await _mediator.Send<PutPomodoroResponse>(request);

        return response;
    }

    [HttpDelete("{id:int}", Name = "DeletePomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeletePomodoroResponse))]
    public async Task<DeletePomodoroResponse> DeletePomodoro(int id)
    {
        var request = new DeletePomodoroRequest();

        var response = await _mediator.Send<DeletePomodoroResponse>(request);

        return response;
    }
}
