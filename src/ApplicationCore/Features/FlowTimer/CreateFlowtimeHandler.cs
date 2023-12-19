using Microsoft.Extensions.Logging;
using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class CreateFlowtimeHandler : IRequestHandler<CreateFlowtimeRequest, CreateFlowtimeResponse>
{
    private readonly Repository _repository;

    private readonly ILogger<CreateFlowtimeHandler> _logger;

    public CreateFlowtimeHandler(Repository repository, ILogger<CreateFlowtimeHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CreateFlowtimeResponse> Handle(CreateFlowtimeRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle Create Flowtime Request: {CorrelationId}", request.GetCorrelationId());

        var task = new Models.TaskManagement.Tasks.Task(request.TaskDescription);

        await _repository.Save(task, -1);

        var flowtime = new Flowtime(task);

        await _repository.Save(flowtime, -1);

        var response = new CreateFlowtimeResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
