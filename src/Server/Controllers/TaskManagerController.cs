using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pomodorium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskManagerController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<TaskManagerController> _logger;

    public TaskManagerController(
        IMediator mediator,
        ILogger<TaskManagerController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }
}
