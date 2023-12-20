using Microsoft.AspNetCore.Mvc;
using Pomodorium.Models;
using Pomodorium.Repositories;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ITfsIntegrationRepository _tfsIntegrationRepository;

    private readonly ITrelloIntegrationRepository _trelloIntegrationRepository;

    private readonly ILogger<SettingsController> _logger;

    public SettingsController(
        ITfsIntegrationRepository tfsIntegrationRepository,
        ITrelloIntegrationRepository trelloIntegrationRepository,
        ILogger<SettingsController> logger)
    {
        _tfsIntegrationRepository = tfsIntegrationRepository;

        _trelloIntegrationRepository = trelloIntegrationRepository;

        _logger = logger;
    }

    [HttpGet("TfsIntegration")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TfsIntegration>))]
    public async Task<IActionResult> GetTfsIntegrationList([FromQuery] TfsIntegration criteria)
    {
        var tfsIntegrationList = await _tfsIntegrationRepository.GetTfsIntegrationList(criteria);

        return Ok(tfsIntegrationList);
    }

    [HttpPost("TfsIntegration")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TfsIntegration))]
    public async Task<IActionResult> PostTfsIntegration(TfsIntegration tfsIntegration)
    {
        var tfsIntegrationCreated = await _tfsIntegrationRepository.CreateTfsIntegration(tfsIntegration);

        return CreatedAtAction(nameof(GetTfsIntegration), new { id = tfsIntegrationCreated.Id }, tfsIntegrationCreated);
    }

    [HttpGet("TfsIntegration/{id}")]
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

    [HttpPut("TfsIntegration/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TfsIntegration))]
    public async Task<IActionResult> PutTfsIntegration(Guid id, TfsIntegration tfsIntegration)
    {
        tfsIntegration.Id = id;

        var tfsIntegrationUpdated = await _tfsIntegrationRepository.UpdateTfsIntegration(tfsIntegration);

        return Ok(tfsIntegrationUpdated);
    }

    [HttpDelete("TfsIntegration/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(TfsIntegration))]
    public async Task<IActionResult> DeleteTfsIntegration(Guid id)
    {
        await _tfsIntegrationRepository.DeleteTfsIntegration(id);

        return NoContent();
    }

    [HttpGet("TrelloIntegration")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrelloIntegration>))]
    public async Task<IActionResult> GetTrelloIntegrationList([FromQuery] TrelloIntegration criteria)
    {
        var trelloIntegrationList = await _trelloIntegrationRepository.GetTrelloIntegrationList(criteria);

        return Ok(trelloIntegrationList);
    }

    [HttpPost   ("TrelloIntegration")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrelloIntegration))]
    public async Task<IActionResult> PostTrelloIntegration(TrelloIntegration trelloIntegration)
    {
        var trelloIntegrationCreated = await _trelloIntegrationRepository.CreateTrelloIntegration(trelloIntegration);

        return CreatedAtAction(nameof(GetTrelloIntegration), new { id = trelloIntegrationCreated.Id }, trelloIntegrationCreated);
    }

    [HttpGet("TrelloIntegration/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrelloIntegration>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTrelloIntegration(Guid id)
    {
        var trelloIntegration = await _trelloIntegrationRepository.GetTrelloIntegration(id);

        if (trelloIntegration == null)
        {
            return NotFound();
        }

        return Ok(trelloIntegration);
    }

    [HttpPut("TrelloIntegration/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrelloIntegration))]
    public async Task<IActionResult> PutTrelloIntegration(Guid id, TrelloIntegration trelloIntegration)
    {
        trelloIntegration.Id = id;

        var trelloIntegrationUpdated = await _trelloIntegrationRepository.UpdateTrelloIntegration(trelloIntegration);

        return Ok(trelloIntegrationUpdated);
    }

    [HttpDelete("TrelloIntegration/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(TrelloIntegration))]
    public async Task<IActionResult> DeleteTrelloIntegration(Guid id)
    {
        await _trelloIntegrationRepository.DeleteTrelloIntegration(id);

        return NoContent();
    }
}
