using Microsoft.Extensions.Logging;
using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeCreationHandler : IRequestHandler<FlowtimeCreationRequest, FlowtimeCreationResponse>
{
    private readonly Repository _repository;

    private readonly ILogger<FlowtimeCreationHandler> _logger;

    public FlowtimeCreationHandler(Repository repository, ILogger<FlowtimeCreationHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<FlowtimeCreationResponse> Handle(FlowtimeCreationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle Create Flowtime Request: {CorrelationId}", request.GetCorrelationId());

        var task = new Models.TaskManagement.Tasks.Task(request.TaskDescription);

        await _repository.Save(task, -1);

        var flowtime = new Flowtime(task);

        await _repository.Save(flowtime, -1);

        var response = new FlowtimeCreationResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
