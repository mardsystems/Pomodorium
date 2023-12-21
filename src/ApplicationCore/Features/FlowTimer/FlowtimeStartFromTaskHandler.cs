using Pomodorium.Models.FlowtimeTechnique;

namespace Pomodorium.Features.FlowTimer;

public class FlowtimeStartFromTaskHandler : IRequestHandler<FlowtimeStartFromTaskRequest, FlowtimeStartFromTaskResponse>
{
    private readonly Repository _repository;

    public FlowtimeStartFromTaskHandler(Repository repository)
    {
        _repository = repository;
    }

    public async Task<FlowtimeStartFromTaskResponse> Handle(FlowtimeStartFromTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetAggregateById<Models.TaskManagement.Tasks.Task>(request.TaskId);

        var flowtime = new Flowtime(task);

        var now = DateTime.Now;

        flowtime.Start(now);

        await _repository.Save(flowtime, -1);

        var response = new FlowtimeStartFromTaskResponse(request.GetCorrelationId())
        {
            FlowtimeVersion = flowtime.Version
        };

        return response;
    }
}
