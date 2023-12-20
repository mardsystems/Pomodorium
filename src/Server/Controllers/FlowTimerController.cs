using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Pomodorium.Features.FlowTimer;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class FlowTimerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<FlowTimerController> _logger;

    public FlowTimerController(IMediator mediator, ILogger<FlowTimerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpPost("GetFlows")]
    public async Task<GetFlowsResponse> GetFlows(GetFlowsRequest request)
    {
        var response = await _mediator.Send<GetFlowsResponse>(request);

        return response;
    }

    [HttpPost("GetFlowtime")]
    public async Task<GetFlowtimeResponse> GetFlowtime(GetFlowtimeRequest request)
    {
        var response = await _mediator.Send<GetFlowtimeResponse>(request);

        return response;
    }

    [HttpPost("CreateFlowtime")]
    public async Task<CreateFlowtimeResponse> CreateFlowtime(CreateFlowtimeRequest request)
    {
        var response = await _mediator.Send<CreateFlowtimeResponse>(request);

        return response;
    }

    [HttpPost("CreateFlowtimeFromTask")]
    public async Task<CreateFlowtimeFromTaskResponse> CreateFlowtimeFromTask(CreateFlowtimeFromTaskRequest request)
    {
        var response = await _mediator.Send<CreateFlowtimeFromTaskResponse>(request);

        return response;
    }

    [HttpPost("StartFlowtime")]
    public async Task<StartFlowtimeResponse> StartFlowtime(StartFlowtimeRequest request)
    {
        var response = await _mediator.Send<StartFlowtimeResponse>(request);

        return response;
    }

    [HttpPost("StartFlowtimeFromTask")]
    public async Task<StartFlowtimeFromTaskResponse> StartFlowtimeFromTask(StartFlowtimeFromTaskRequest request)
    {
        var response = await _mediator.Send<StartFlowtimeFromTaskResponse>(request);

        return response;
    }

    [HttpPost("InterruptFlowtime")]
    public async Task<InterruptFlowtimeResponse> InterruptFlowtime(InterruptFlowtimeRequest request)
    {
        var response = await _mediator.Send<InterruptFlowtimeResponse>(request);

        return response;
    }

    [HttpPost("StopFlowtime")]
    public async Task<StopFlowtimeResponse> StopFlowtime(StopFlowtimeRequest request)
    {
        var response = await _mediator.Send<StopFlowtimeResponse>(request);

        return response;
    }

    [HttpPost("ArchiveFlowtime")]
    public async Task<ArchiveFlowtimeResponse> ArchiveFlowtime(ArchiveFlowtimeRequest request)
    {
        var response = await _mediator.Send<ArchiveFlowtimeResponse>(request);

        return response;
    }
}
