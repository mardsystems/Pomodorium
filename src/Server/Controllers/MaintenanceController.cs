using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Pomodorium.Features.Maintenance;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class MaintenanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaintenanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("IndexRebuild")]
    public async Task<IndexRebuildResponse> PostIndexRebuild(IndexRebuildRequest request)
    {
        var response = await _mediator.Send<IndexRebuildResponse>(request);

        return response;
    }
}
