using Microsoft.AspNetCore.Mvc;
using Pomodorium.Features.Settings;
using Pomodorium.Models;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly MongoDBTfsIntegrationCollection _tfsIntegrationRepository;

    private readonly ILogger<SettingsController> _logger;

    public SettingsController(
        MongoDBTfsIntegrationCollection tfsIntegrationRepository,
        ILogger<SettingsController> logger)
    {
        _tfsIntegrationRepository = tfsIntegrationRepository;

        _logger = logger;
    }

    [HttpGet("TfsIntegration", Name = "GetTfsIntegrationList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TfsIntegration>))]
    public async Task<IActionResult> GetTfsIntegrationList([FromQuery] TfsIntegration criteria)
    {
        var tfsIntegrationList = await _tfsIntegrationRepository.GetTfsIntegrationList(criteria);

        return Ok(tfsIntegrationList);
    }

    [HttpPost("TfsIntegration", Name = "PostTfsIntegration")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TfsIntegration))]
    public async Task<IActionResult> PostTfsIntegration(TfsIntegration tfsIntegration)
    {
        var tfsIntegrationCreated = await _tfsIntegrationRepository.CreateTfsIntegration(tfsIntegration);

        return CreatedAtAction(nameof(GetTfsIntegration), tfsIntegrationCreated);
    }

    [HttpGet("TfsIntegration/{id}", Name = "GetTfsIntegration")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TfsIntegration>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTfsIntegration(Guid id)
    {
        var tfsIntegration = await _tfsIntegrationRepository.GetTfsIntegration(id);

        if (tfsIntegration == null)
        {
            return NotFound();
        }

        return Ok(tfsIntegration);
    }

    [HttpPut("TfsIntegration/{id}", Name = "PutTfsIntegration")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TfsIntegration))]
    public async Task<IActionResult> PutTfsIntegration(Guid id, TfsIntegration tfsIntegration)
    {
        tfsIntegration.Id = id;

        var tfsIntegrationUpdated = await _tfsIntegrationRepository.UpdateTfsIntegration(tfsIntegration);

        return Ok(tfsIntegrationUpdated);
    }

    [HttpDelete("TfsIntegration/{id}", Name = "DeleteTfsIntegration")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TfsIntegration))]
    public async Task<IActionResult> DeleteTfsIntegration(Guid id)
    {
        await _tfsIntegrationRepository.DeleteTfsIntegration(id);

        return NoContent();
    }
}
