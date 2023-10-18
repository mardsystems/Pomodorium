using Microsoft.AspNetCore.Mvc;
using Pomodorium.Data;
using Pomodorium.Modules.Pomodori;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PomodoriController : ControllerBase
{
    private readonly PomodoroRepository _pomodoroRepository;

    private readonly PomodoriumDbContext _db;

    private readonly ILogger<PomodoriController> _logger;

    public PomodoriController(
        PomodoroRepository pomodoroRepository,
        PomodoriumDbContext db,
        ILogger<PomodoriController> logger)
    {
        _pomodoroRepository = pomodoroRepository;

        _db = db;

        _logger = logger;
    }

    [HttpGet("", Name = "GetPomodori")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPomodoriResponse))]
    public async Task<GetPomodoriResponse> GetPomodori([FromQuery] GetPomodoriRequest request)
    {
        var pomodoroQueryItems = _db.PomodoroQueryItems
            .ToArray();

        var response = new GetPomodoriResponse(request.GetCorrelationId()) { PomodoroQueryItems = pomodoroQueryItems };

        return response;
    }

    [HttpPost("", Name = "PostPomodoro")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostPomodoroResponse))]
    public async Task<PostPomodoroResponse> PostPomodoro(PostPomodoroRequest request)
    {
        var correlationId = request.GetCorrelationId();

        var pomodoroId = new PomodoroId(correlationId.ToString());

        var pomodoro = new Pomodoro(pomodoroId, request.StartDateTime, request.Description);

        await _pomodoroRepository.Add(pomodoro);
        
        var response = new PostPomodoroResponse(request.GetCorrelationId()) { };

        return response;
    }

    [HttpGet("{id:int}", Name = "GetPomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPomodoroResponse))]
    public async Task<IActionResult> GetPomodoro(int id)
    {
        var request = new GetPomodoroRequest();

        var pomodoroDetails = _db.PomodoroDetails
            .FirstOrDefault(x => x.Id == id);

        if (pomodoroDetails == null)
        {
            return NotFound();
        }

        var response = new GetPomodoroResponse(request.GetCorrelationId()) { PomodoroDetails = pomodoroDetails };

        return Ok(response);
    }

    [HttpPut("{id:int}", Name = "PutPomodoro")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PutPomodoroResponse))]
    public async Task<PutPomodoroResponse> PutPomodoro(string id, PutPomodoroRequest request)
    {
        var pomodoroId = new PomodoroId(id);

        var pomodoro = await _pomodoroRepository.GetPomodoroById(pomodoroId);

        //if (pomodoro == null)
        //{
        //    return NotFound();
        //}

        pomodoro.ChangeDescription(request.Description);

        await _pomodoroRepository.Update(pomodoro);

        var response = new PutPomodoroResponse(request.GetCorrelationId()) { };

        return response;
    }

    //[HttpDelete("{id:int}", Name = "DeletePomodoro")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeletePomodoroResponse))]
    //public async Task<IActionResult> DeletePomodoro(int id)
    //{
    //    var request = new DeletePomodoroRequest();

    //    var pomodoro = await _pomodoroRepository.GetPomodoroById(id);

    //    if (pomodoro == null)
    //    {
    //        return NotFound();
    //    }

    //    await _pomodoroRepository.Delete(pomodoro);

    //    var pomodoroDetails = _mapper.Map<PomodoroDetails>(pomodoro);

    //    var response = new DeletePomodoroResponse(request.GetCorrelationId()) { PomodoroDetails = pomodoroDetails };

    //    return Ok(response);
    //}
}
