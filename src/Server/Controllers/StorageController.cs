using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.DomainModel.Storage;

namespace Pomodorium.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class StorageController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<StorageController> _logger;

    public StorageController(
        IMediator mediator,
        ILogger<StorageController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    [HttpGet("Events", Name = "GetEvents")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventsResponse))]
    public async Task<GetEventsResponse> GetEvents([FromQuery] GetEventsRequest request)
    {
        var response = await _mediator.Send<GetEventsResponse>(request);

        return response;
    }
}
