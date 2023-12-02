using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pomodorium.Features.Settings;
using Pomodorium.Features.TaskSynchronizer;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<SettingsController> _logger;

    public SettingsController(
        IMediator mediator,
        ILogger<SettingsController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpGet("TfsIntegration", Name = "GetTfsIntegrationList")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(GetTfsIntegrationListResponse))]
    public async Task<GetTfsIntegrationListResponse> SyncTasksFromTfs([FromQuery] GetTfsIntegrationListRequest request)
    {
        var response = await _mediator.Send<GetTfsIntegrationListResponse>(request);

        return response;
    }

    [HttpPost("TfsIntegration/Create", Name = "PostCreateTfsIntegration")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(CreateTfsIntegrationResponse))]
    public async Task<CreateTfsIntegrationResponse> PostCreateTfsIntegration(CreateTfsIntegrationRequest request)
    {
        var response = await _mediator.Send<CreateTfsIntegrationResponse>(request);

        return response;
    }
}
